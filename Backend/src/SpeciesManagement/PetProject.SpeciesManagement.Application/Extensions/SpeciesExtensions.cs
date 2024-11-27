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
            SpeciesId.Create(dto.Id),
            SpeciesName.Create(dto.Name).Value,
            dto.Breeds.Select(b => b.ToEntity()).ToList());
        return species;
    }

    public static Breed ToEntity(this BreedDto dto)
    {
        var breed = new Breed(
            BreedId.Create(dto.Id),
            BreedName.Create(dto.Name).Value
        );
        return breed;
    }
}