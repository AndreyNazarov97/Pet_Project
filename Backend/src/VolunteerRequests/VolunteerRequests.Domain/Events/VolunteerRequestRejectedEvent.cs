using PetProject.SharedKernel.Interfaces;

namespace VolunteerRequests.Domain.Events;

public record VolunteerRequestRejectedEvent : IDomainEvent
{
    public required long UserId { get; init; }
}