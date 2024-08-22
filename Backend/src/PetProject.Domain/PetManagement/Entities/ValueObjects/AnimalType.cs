using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.PetManagement.Entities.ValueObjects;

public class AnimalType : ValueObject
{
    private AnimalType(){}

    private AnimalType(SpeciesId speciesId, BreedId breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    
    public SpeciesId SpeciesId { get;  }
    public BreedId BreedId { get;  }
    
    public static Result<AnimalType> Create(SpeciesId speciesId, BreedId breedId)
    {
        
        return new AnimalType(speciesId, breedId);
    }
    
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return SpeciesId;
        yield return BreedId;
    }
}