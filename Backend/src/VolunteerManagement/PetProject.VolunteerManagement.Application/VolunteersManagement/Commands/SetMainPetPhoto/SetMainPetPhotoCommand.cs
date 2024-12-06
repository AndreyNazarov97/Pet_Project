using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SetMainPetPhoto;

public class SetMainPetPhotoCommand : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
    public required string Path { get; init; }
}