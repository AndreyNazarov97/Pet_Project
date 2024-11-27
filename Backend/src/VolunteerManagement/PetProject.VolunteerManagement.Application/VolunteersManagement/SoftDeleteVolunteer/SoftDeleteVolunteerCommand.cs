using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.SoftDeleteVolunteer;

public record SoftDeleteVolunteerCommand() : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerId { get; init; } 
}