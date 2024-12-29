using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos.Discussions;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Queries.GetByRelationId;

public record GetByRelationIdQuery : IRequest<Result<DiscussionDto, ErrorList>>
{
    public Guid RelationId { get; init; }
}