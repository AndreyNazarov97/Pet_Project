using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands.CloseDiscussion;

public record CloseDiscussionCommand : IRequest<UnitResult<ErrorList>>
{
    public required Guid DiscussionId { get; init; }
    public required long UserId { get; init; }
}