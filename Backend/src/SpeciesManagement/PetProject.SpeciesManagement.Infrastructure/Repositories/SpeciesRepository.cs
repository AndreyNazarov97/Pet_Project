using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PetProject.Core.Database;
using PetProject.Core.Database.Models;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SpeciesManagement.Application.Repository;
using PetProject.SpeciesManagement.Domain.Aggregate;
using PetProject.SpeciesManagement.Infrastructure.DbContexts;

namespace PetProject.SpeciesManagement.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly SpeciesDbContext _context;

    public SpeciesRepository(
        SpeciesDbContext context)
    {
        _context = context;
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