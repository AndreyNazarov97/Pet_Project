using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.AddPetPhoto;

public record AddPetPhotoCommand: IRequest<Result<FilePath[], ErrorList>>
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
    public required List<FileDto> Photos { get; init; }
}