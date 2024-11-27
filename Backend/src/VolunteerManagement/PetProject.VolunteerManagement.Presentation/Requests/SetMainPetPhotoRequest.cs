using PetProject.VolunteerManagement.Application.VolunteersManagement.SetMainPetPhoto;

namespace PetProject.VolunteerManagement.Presentation.Requests;

public record SetMainPetPhotoRequest(string Path)
{
    public SetMainPetPhotoCommand ToCommand(Guid volunteerId, Guid petId) => new()
    {
        VolunteerId = volunteerId,
        PetId = petId,
        Path = Path
    };
}