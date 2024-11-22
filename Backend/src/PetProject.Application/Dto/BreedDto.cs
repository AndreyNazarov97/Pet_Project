using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.Dto;

public record BreedDto(Guid Id, string Name)
{
    public Breed ToEntity()
    {
        var breed = new Breed(
            BreedId.Create(Id),
            BreedName.Create(Name).Value
        );
        
        return breed;
    }
};
