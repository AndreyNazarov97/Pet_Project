using PetProject.Application.Dto;
using PetProject.Application.VolunteersManagement.UpdatePet;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record UpdatePetRequest(PetDto PetInfo)
{
    public UpdatePetCommand ToCommand(Guid volunteerId, Guid petId) => new()
    {
        VolunteerId = volunteerId,
        PetId = petId,
        PetInfo = PetInfo
    };
}