using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;
using PetProject.VolunteerManagement.Domain.Enums;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.ChangePetStatus;

public record ChangePetStatusCommand : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
    public required HelpStatus HelpStatus { get; init; }
}