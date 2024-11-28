using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Core.Dtos;

public record SpeciesDto()
{
    public required Guid Id { get; init; } 
    public required string Name { get; init; }
    public List<BreedDto> Breeds { get; init; } = [];
};
