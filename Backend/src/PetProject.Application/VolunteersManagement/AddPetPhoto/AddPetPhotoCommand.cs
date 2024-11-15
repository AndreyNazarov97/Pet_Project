using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.AddPetPhoto;

public record AddPetPhotoCommand: IRequest<Result<string, Error>>
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
    public required List<FileDto> Photos { get; init; }
}