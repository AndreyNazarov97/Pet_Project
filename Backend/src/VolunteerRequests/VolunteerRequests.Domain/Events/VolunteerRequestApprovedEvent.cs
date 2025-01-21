using PetProject.SharedKernel.Interfaces;
using PetProject.SharedKernel.Shared.EntityIds;

namespace VolunteerRequests.Domain.Events;

public record VolunteerRequestApprovedEvent : IDomainEvent
{
    public required VolunteerRequestId VolunteerRequestId { get; init; }
    public required long UserId { get; init; }
    
}