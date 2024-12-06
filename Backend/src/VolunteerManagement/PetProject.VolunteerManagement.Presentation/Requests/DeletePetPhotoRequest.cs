using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.DeletePetPhoto;

namespace PetProject.VolunteerManagement.Presentation.Requests;

public record DeletePetPhotoRequest(string FilePath)
{
    public DeletePetPhotoCommand ToCommand(Guid volunteerId, Guid petId) => new()
    {
        VolunteerId = volunteerId,
        PetId = petId,
        FilePath = FilePath
    };
}