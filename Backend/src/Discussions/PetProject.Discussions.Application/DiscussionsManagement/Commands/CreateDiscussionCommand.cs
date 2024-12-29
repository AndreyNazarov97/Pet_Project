using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands;

public record CreateDiscussionCommand : IRequest<Result<Discussion, ErrorList>>
{
    public required Guid RelationId { get; init; }
    public required long FirstMemberId { get; init; }
    public required long SecondMemberId { get; init; }
}