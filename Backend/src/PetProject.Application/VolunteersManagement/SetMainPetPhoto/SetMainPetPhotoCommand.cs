using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.SetMainPetPhoto;

public class SetMainPetPhotoCommand : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
    public required string Path { get; init; }
}