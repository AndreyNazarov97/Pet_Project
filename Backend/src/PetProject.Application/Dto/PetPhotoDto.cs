namespace PetProject.Application.Dto;

public record PetPhotoDto
{
    public required string Path { get; init; }
    public bool IsMain { get; init; }
}