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

    public async Task<SpeciesDto[]> Query(SpeciesQueryModel query,
        CancellationToken cancellationToken = default)
    {
        var sqlQuery = """
                       select 
                           s.id,
                           s.species_name,
                           b.id,
                           b.breed_name
                       from 
                           species as s 
                       left join 
                           breeds as b on b.species_id = s.id AND b.is_deleted = false
                       """;

        var conditions = new List<string>(["s.is_deleted = false"]);
        var param = new DynamicParameters();

        if (string.IsNullOrEmpty(query.SpeciesName) == false)
        {
            conditions.Add("s.species_name = @SpeciesName");
            param.Add("SpeciesName", query.SpeciesName);
        }
        
        if (string.IsNullOrEmpty(query.BreedName) == false)
        {
            conditions.Add("b.breed_name = @BreedName");
            param.Add("BreedName", query.BreedName);
        }

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

        var speciesSet = new HashSet<SpeciesDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var speciesId = reader.GetGuid(0);
            var speciesName = reader.GetString(1);
            var breedId = reader.GetGuid(2);
            var breedName = reader.GetString(3);

            var species = speciesSet.FirstOrDefault(s => s.Name == speciesName);
            if (species == null)
            {
                species = new SpeciesDto(speciesId, speciesName, []);

                speciesSet.Add(species);
            }

            var breedDto = new BreedDto(breedId, breedName);

            species.Breeds.Add(breedDto);
        }

        return speciesSet.ToArray();
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