namespace PetProject.Core.Dtos.Discussions;

public record MembersDto
{
    public required Guid Id { get; init; }
    public required long FirstMemberId { get; init; }
    public required long SecondMemberId { get; init; }
}