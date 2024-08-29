using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Domain.Species;

public class Breed : Shared.Entity<BreedId>
{
    public BreedName BreedName { get; }

    protected Breed(BreedId id) : base(id) { }

    public Breed(BreedId id, BreedName breedName) : base(id)
    {
        BreedName = breedName;
    }
}