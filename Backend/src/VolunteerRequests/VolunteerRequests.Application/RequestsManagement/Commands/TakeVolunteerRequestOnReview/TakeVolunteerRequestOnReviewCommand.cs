using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;

public record TakeVolunteerRequestOnReviewCommand : IRequest<Result<DiscussionId ,ErrorList>>
{
    public required Guid VolunteerRequestId { get; init; }
    public required long AdminId { get; init; }
}