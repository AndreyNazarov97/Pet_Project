using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.SpeciesManagement;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Infrastructure.Postgres.Repositories;

public class SpeciesRepository(PetProjectDbContext context) : ISpeciesRepository
{
    public async Task<Result<List<Species>, Error>> GetAll(CancellationToken cancellationToken = default)
    {
        var species = await context.Species.ToListAsync(cancellationToken);
        
        return species;
    }
    
    public async Task<Result<Species, Error>> Get(SpeciesId id, CancellationToken cancellationToken = default)
    {
        var species = await context.Species.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (species == null)
            return Errors.General.NotFound(id.Id);
        
        return species;
    }

    public async Task<Result<Species, Error>> GetByName(SpeciesName name, CancellationToken cancellationToken = default)
    {
        var species = await context.Species.FirstOrDefaultAsync(s => s.Name == name, cancellationToken);

        if (species == null)
            return Errors.General.NotFound();
        
        return species;
    }

    public async Task<Result<SpeciesId, Error>> Save(Species species, CancellationToken cancellationToken = default)
    {
        context.Species.Attach(species);
        await context.SaveChangesAsync(cancellationToken);
        return species.Id;
    }

    public async Task<Result<SpeciesId, Error>> Add(Species species, CancellationToken cancellationToken = default)
    {
        await context.Species.AddAsync(species, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return species.Id;
    }

    public async Task<Result<SpeciesId, Error>> Delete(SpeciesId id, CancellationToken cancellationToken = default)
    {
        var species = await context.Species.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (species == null)
            return Errors.General.NotFound(id.Id);
        
        context.Species.Remove(species);
        await context.SaveChangesAsync(cancellationToken);

        return species.Id;
    }
}