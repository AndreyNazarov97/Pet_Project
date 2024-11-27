using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.VolunteerManagement.Domain.ValueObjects
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