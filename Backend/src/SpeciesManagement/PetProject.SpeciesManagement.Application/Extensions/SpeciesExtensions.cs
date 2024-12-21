using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.SpeciesManagement.Domain.Aggregate;
using PetProject.SpeciesManagement.Domain.Entities;

namespace PetProject.SpeciesManagement.Application.Extensions;

public static class SpeciesExtensions
{
    public static Species ToEntity(this SpeciesDto dto)
    {
        var species = new Species(
            SpeciesId.Create(dto.SpeciesId),
            SpeciesName.Create(dto.SpeciesName).Value,
            dto.Breeds.Select(b => b.ToEntity()).ToList());
        return species;
    }

    public static Breed ToEntity(this BreedDto dto)
    {
        var breed = new Breed(
            BreedId.Create(dto.BreedId),
            BreedName.Create(dto.BreedName).Value
        );
        return breed;
    }
}