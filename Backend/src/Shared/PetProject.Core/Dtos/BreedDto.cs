namespace PetProject.Core.Dtos;

public record BreedDto()
{
    public required Guid BreedId { get; init; } 
    public required string BreedName { get; init; }
};
