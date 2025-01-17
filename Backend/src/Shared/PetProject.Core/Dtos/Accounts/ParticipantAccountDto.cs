namespace PetProject.Core.Dtos.Accounts;

public record ParticipantAccountDto
{
    public required long ParticipantAccountId { get; init; }
    public required long UserId { get; init; }
}