using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;
using PetProject.Domain.VolunteerManagement.Enums;

namespace PetProject.Application.VolunteersManagement.ChangePetStatus;

public record ChangePetStatusCommand : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
    public required HelpStatus HelpStatus { get; init; }
}