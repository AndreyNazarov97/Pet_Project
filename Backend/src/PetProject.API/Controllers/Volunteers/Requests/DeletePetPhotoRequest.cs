using PetProject.Application.VolunteersManagement.DeletePetPhoto;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record DeletePetPhotoRequest(string FilePath)
{
    public DeletePetPhotoCommand ToCommand(Guid volunteerId, Guid petId) => new()
    {
        VolunteerId = volunteerId,
        PetId = petId,
        FilePath = FilePath
    };
}