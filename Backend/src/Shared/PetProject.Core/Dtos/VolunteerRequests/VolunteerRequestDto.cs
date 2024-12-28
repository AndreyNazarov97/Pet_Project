using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Core.Dtos.VolunteerRequests;

public record VolunteerRequestDto
{
    public required Guid Id { get; init; }
    public required VolunteerInfoDto VolunteerInfo { get; set; }
    public required string RequestStatus { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public long? AdminId { get; init; }
    public required long UserId { get; init; }
    public Guid? DiscussionId { get; init; }
    public string? RejectionComment { get; init; }
}