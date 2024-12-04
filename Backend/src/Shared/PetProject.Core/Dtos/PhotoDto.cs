namespace PetProject.Core.Dtos;

public record PhotoDto
{
    public required string Path { get; init; }
    public bool IsMain { get; init; }
}