using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.DeleteVolunteer;

public record DeleteVolunteerCommand() : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerId { get; init; } 
}