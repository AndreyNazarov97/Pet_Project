using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Domain.VolunteerManagement.ValueObjects
{
    public record AnimalType
    {
        public SpeciesId SpeciesId { get;  }
        public BreedId BreedId { get;  }
        
        public AnimalType(SpeciesId speciesId, BreedId breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }
    }
}