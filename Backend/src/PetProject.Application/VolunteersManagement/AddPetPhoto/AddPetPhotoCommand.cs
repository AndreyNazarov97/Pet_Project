using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.VolunteersManagement.AddPetPhoto;

public record AddPetPhotoCommand: IRequest<Result<FilePath[], ErrorList>>
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
    public required List<FileDto> Photos { get; init; }
}