using PetProject.Application.VolunteersManagement.SetMainPetPhoto;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record SetMainPetPhotoRequest(string Path)
{
    public SetMainPetPhotoCommand ToCommand(Guid volunteerId, Guid petId) => new()
    {
        VolunteerId = volunteerId,
        PetId = petId,
        Path = Path
    };
}