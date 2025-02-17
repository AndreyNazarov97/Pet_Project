namespace PetProject.Core.Dtos;

public record PhotoDto
{
    public required Guid FileId { get; init; }
    public bool IsMain { get; init; }
}