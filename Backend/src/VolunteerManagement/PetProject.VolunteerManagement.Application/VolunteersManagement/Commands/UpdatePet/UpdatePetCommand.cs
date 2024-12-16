using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.UpdatePet;

public record UpdatePetCommand : IRequest<Result<Guid, ErrorList>>
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
    public required PetDto PetInfo { get; init; }
}