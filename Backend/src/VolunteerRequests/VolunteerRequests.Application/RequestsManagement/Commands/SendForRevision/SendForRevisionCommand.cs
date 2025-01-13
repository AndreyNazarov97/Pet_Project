using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace VolunteerRequests.Application.RequestsManagement.Commands.SendForRevision;

public record  SendForRevisionCommand : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerRequestId { get; init; }
    public required long AdminId { get; init; }
    public required string RejectionComment { get; init; }
}