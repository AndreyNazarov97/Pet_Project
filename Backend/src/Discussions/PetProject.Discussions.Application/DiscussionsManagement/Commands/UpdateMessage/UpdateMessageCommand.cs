using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands.UpdateMessage;

public record UpdateMessageCommand : IRequest<Result<Guid, ErrorList>>
{
    public required long UserId { get; init; }
    public required Guid DiscussionId { get; init; }
    public required Guid MessageId { get; init; }
    public required string Text { get; init; }
}