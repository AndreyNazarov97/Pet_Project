using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities.ValueObjects;

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