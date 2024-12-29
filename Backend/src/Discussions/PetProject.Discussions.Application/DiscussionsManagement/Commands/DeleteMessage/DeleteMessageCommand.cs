using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands.DeleteMessage;

public record DeleteMessageCommand : IRequest<UnitResult<ErrorList>>
{
    public required long UserId { get; init; }
    public required Guid DiscussionId { get; init; }
    public required Guid MessageId { get; init; }
}