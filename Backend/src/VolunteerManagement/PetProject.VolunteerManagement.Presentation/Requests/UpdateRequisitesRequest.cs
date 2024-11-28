using PetProject.Core.Dtos;
using PetProject.VolunteerManagement.Application.VolunteersManagement.UpdateRequisites;

namespace PetProject.VolunteerManagement.Presentation.Requests;

public record UpdateRequisitesRequest(IEnumerable<RequisiteDto> Requisites)
{
    public UpdateRequisitesCommand ToCommand(Guid volunteerId) => new()
    {
        VolunteerId = volunteerId,
        Requisites = Requisites
    };
}