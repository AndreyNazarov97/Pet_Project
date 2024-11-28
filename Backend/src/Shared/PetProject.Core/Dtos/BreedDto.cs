using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Core.Dtos;

public record BreedDto()
{
    public required Guid Id { get; init; } 
    public required string Name { get; init; }
};
