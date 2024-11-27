namespace PetProject.Core.Dtos;

public record RequisiteDto()
{
    public required string Title { get; init; } 
    public required string Description { get; init; } 
}