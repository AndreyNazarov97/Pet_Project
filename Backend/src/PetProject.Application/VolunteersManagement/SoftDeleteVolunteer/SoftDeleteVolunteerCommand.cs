using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.SoftDeleteVolunteer;

public record SoftDeleteVolunteerCommand() : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerId { get; init; } 
}