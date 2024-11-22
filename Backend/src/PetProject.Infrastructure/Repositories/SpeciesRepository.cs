﻿using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Application.SpeciesManagement;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;
using PetProject.Infrastructure.Postgres.Abstractions;

namespace PetProject.Infrastructure.Postgres.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly PetProjectDbContext _context;
    private readonly IPostgresConnectionFactory _connectionFactory;

    public SpeciesRepository(
        PetProjectDbContext context,
        IPostgresConnectionFactory connectionFactory)
    {
        _context = context;
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<List<Species>, Error>> GetAll(CancellationToken cancellationToken = default)
    {
        var species = await _context.Species.ToListAsync(cancellationToken);

        return species;
    }

    public async Task<Result<Species, Error>> Get(SpeciesId id, CancellationToken cancellationToken = default)
    {
        var species = await _context.Species.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (species == null)
            return Errors.General.NotFound(id.Id);

        return species;
    }

    public async Task<Result<Species[], Error>> Query(SpeciesQueryModel query,
        CancellationToken cancellationToken = default)
    {
        if (query.IsEmpty())
            return Array.Empty<Species>();

        var sqlQuery = """
                       select 
                           s.id,
                           s.species_name,
                           b.id,
                           b.breed_name
                       from 
                           species as s 
                       left join 
                           breeds as b on b.species_id = s.id
                       """;

        var conditions = new List<string>(["1=1"]);
        var param = new DynamicParameters();

        if (query.SpeciesIds is { Length: > 0 })
        {
            conditions.Add("s.id = any(@SpeciesIds)");
            param.Add("SpeciesIds", query.SpeciesIds);
        }

        if (query.BreedIds is { Length: > 0 })
        {
            conditions.Add("b.id = any(@BreedIds)");
            param.Add("BreedIds", query.BreedIds);
        }

        sqlQuery += " where " + string.Join(" and ", conditions);

        if (query.Limit > 0)
        {
            sqlQuery += " limit @Limit ";
            param.Add("Limit", query.Limit);
        }

        if (query.Offset > 0)
        {
            sqlQuery += " offset @Offset ";
            param.Add("Offset", query.Offset);
        }

        var command = new CommandDefinition(sqlQuery, param, cancellationToken: cancellationToken);

        await using var connection = _connectionFactory.GetConnection();

        await using var reader = await connection.ExecuteReaderAsync(command);

        var speciesSet = new HashSet<Species>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var speciesId = SpeciesId.Create(reader.GetGuid(0));
            var breedId = BreedId.Create(reader.GetGuid(2));
            var breedName = BreedName.Create(reader.GetString(3)).Value;

            var species = speciesSet.FirstOrDefault(s => s.Id == speciesId);
            if (species == null)
            {
                var speciesName = SpeciesName.Create(reader.GetString(1)).Value;
                species = new Species(speciesId, speciesName, []);
                
                speciesSet.Add(species);
            }

            var breed = new Breed(breedId, breedName);

            species.AddBreeds([breed]);
        }

        return speciesSet.ToArray();
    }


    public async Task<Result<Species, Error>> GetByName(SpeciesName name, CancellationToken cancellationToken = default)
    {
        var species = await _context.Species.FirstOrDefaultAsync(s => s.Name == name, cancellationToken);

        if (species == null)
            return Errors.General.NotFound();

        return species;
    }

    public async Task<Result<SpeciesId, Error>> Save(Species species, CancellationToken cancellationToken = default)
    {
        _context.Species.Attach(species);
        await _context.SaveChangesAsync(cancellationToken);
        return species.Id;
    }

    public async Task<Result<SpeciesId, Error>> Add(Species species, CancellationToken cancellationToken = default)
    {
        await _context.Species.AddAsync(species, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return species.Id;
    }

    public async Task<Result<SpeciesId, Error>> Delete(SpeciesId id, CancellationToken cancellationToken = default)
    {
        var species = await _context.Species.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (species == null)
            return Errors.General.NotFound(id.Id);

        _context.Species.Remove(species);
        await _context.SaveChangesAsync(cancellationToken);

        return species.Id;
    }
}