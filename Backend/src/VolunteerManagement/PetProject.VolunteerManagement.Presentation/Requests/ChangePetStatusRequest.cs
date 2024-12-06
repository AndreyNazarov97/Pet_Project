using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.ChangePetStatus;
using PetProject.VolunteerManagement.Domain.Enums;

namespace PetProject.VolunteerManagement.Presentation.Requests;

public record ChangePetStatusRequest(HelpStatus Status)
{
    public ChangePetStatusCommand ToCommand(Guid volunteerId, Guid petId) => new()
    {
        VolunteerId = volunteerId,
        PetId = petId,
        HelpStatus = Status
    };
}