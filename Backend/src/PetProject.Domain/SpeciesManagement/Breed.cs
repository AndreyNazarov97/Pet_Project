using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Domain.SpeciesManagement;

public class Breed : Shared.Common.Entity<BreedId>
{
    public BreedName BreedName { get; }

    protected Breed(BreedId id) : base(id) { }

    public Breed(BreedId id, BreedName breedName) : base(id)
    {
        BreedName = breedName;
    }
}