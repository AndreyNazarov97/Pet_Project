using PetProject.Application.VolunteersManagement.ChangePetStatus;
using PetProject.Domain.VolunteerManagement.Enums;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record ChangePetStatusRequest(HelpStatus Status)
{
    public ChangePetStatusCommand ToCommand(Guid volunteerId, Guid petId) => new()
    {
        VolunteerId = volunteerId,
        PetId = petId,
        HelpStatus = Status
    };
}