using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.DeletePetPhoto;

public record DeletePetPhotoCommand : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
    public required string FilePath { get; init; }
}