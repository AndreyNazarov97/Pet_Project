using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.SpeciesManagement.Domain.Aggregate;
using PetProject.SpeciesManagement.Domain.Entities;
using PetProject.SpeciesManagement.Infrastructure.DbContexts;

namespace PetProject.SpeciesManagement.Infrastructure.DataSeed;

public class SpeciesSeedService
{
    private readonly SpeciesDbContext _context;

    public SpeciesSeedService(
        SpeciesDbContext context)
    {
        _context = context;
    }
    
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (!_context.Species.Any())
        {
            await SeedSpecies(_context, cancellationToken);
        }
    }

    private async Task SeedSpecies(SpeciesDbContext context, CancellationToken cancellationToken)
    {
        var dogSpecies = new Species(
            SpeciesId.NewId(),
            SpeciesName.Create("Dog").Value,
            new List<Breed>()
            {
                new(
                    BreedId.NewId(),
                    BreedName.Create("Bulldog").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("Poodle").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("Golden Retriever").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("Labrador").Value)
            }
        );

        var catSpecies = new Species(
            SpeciesId.NewId(),
            SpeciesName.Create("Cat").Value,
            new List<Breed>()
            {
                new(
                    BreedId.NewId(),
                    BreedName.Create("Persian").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("Siamese").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("Maine Coon").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("British Shorthair").Value)
            }
        );

        await context.Species.AddRangeAsync([dogSpecies, catSpecies], cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}