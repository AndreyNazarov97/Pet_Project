using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Domain.VolunteerManagement.ValueObjects
{
    public record AnimalType
    {
        public SpeciesName SpeciesName { get;  }
        public BreedName BreedName { get;  }
        
        public AnimalType(SpeciesName speciesName, BreedName breedName)
        {
            SpeciesName = speciesName;
            BreedName = breedName;
        }
    }
}