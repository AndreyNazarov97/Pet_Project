using PetProject.SharedKernel.Interfaces;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Domain.ValueObjects;

namespace VolunteerRequests.Domain.Events;

public record VolunteerRequestTakenOnReview : IDomainEvent
{
    public required VolunteerRequestId VolunteerRequestId { get; init; }

    public required Guid DiscussionId { get; init; }
}