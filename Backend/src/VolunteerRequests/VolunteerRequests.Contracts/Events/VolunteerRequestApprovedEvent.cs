namespace VolunteerRequests.Contracts.Events;

public record VolunteerRequestApprovedEvent
{
    public required string FirstName { get; init; }
    public required string Surname { get; init; }
    public string? Patronymic { get; init; }
    public required string Description { get; init; }
    public required string PhoneNumber { get; init; }
    public required int Experience { get; init; }
}