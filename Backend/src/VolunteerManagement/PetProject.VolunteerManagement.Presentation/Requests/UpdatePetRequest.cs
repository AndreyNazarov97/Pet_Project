using PetProject.Core.Dtos;
using PetProject.VolunteerManagement.Application.VolunteersManagement.UpdatePet;

namespace PetProject.VolunteerManagement.Presentation.Requests;

public record UpdatePetRequest(PetDto PetInfo)
{
    public UpdatePetCommand ToCommand(Guid volunteerId, Guid petId) => new()
    {
        VolunteerId = volunteerId,
        PetId = petId,
        PetInfo = PetInfo
    };
}