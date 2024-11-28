namespace PetProject.Core.Dtos;

public record PetPhotoDto
{
    public required string Path { get; init; }
    public bool IsMain { get; init; }
}