using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;

public record TakeVolunteerRequestOnReviewCommand : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerRequestId { get; init; }
    public required long AdminId { get; init; }
}