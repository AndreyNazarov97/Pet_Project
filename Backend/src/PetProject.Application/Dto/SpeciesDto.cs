using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.Dto;

public record SpeciesDto(Guid Id, string Name, List<BreedDto> Breeds)
{
    public Species ToEntity()
    {
        var species = new Species(
            SpeciesId.Create(Id),
            SpeciesName.Create(Name).Value,
            Breeds.Select(b => b.ToEntity()).ToList());
        return species;
    }
};
