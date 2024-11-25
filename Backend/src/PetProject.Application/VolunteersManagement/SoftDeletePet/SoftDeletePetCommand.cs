using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.SoftDeletePet;

public record SoftDeletePetCommand : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
}