using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.DeletePetPhoto;

namespace PetProject.VolunteerManagement.Presentation.Requests;

public record DeletePetPhotoRequest(Guid FileId)
{
    public DeletePetPhotoCommand ToCommand(Guid volunteerId, Guid petId) => new()
    {
        VolunteerId = volunteerId,
        PetId = petId,
        FileId = FileId
    };
}