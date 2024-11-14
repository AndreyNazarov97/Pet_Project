using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.AddPetPhoto;

public record AddPetPhotoCommand
{
    public required Guid VolunteerId { get; init; }
    public required Guid PetId { get; init; }
    public required List<FileDto> Photos { get; init; }
}