using PetProject.Application.Dto;
using PetProject.Application.Volunteers.UpdateRequisites;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record UpdateRequisitesRequest(IEnumerable<RequisiteDto> Requisites)
{
    public UpdateRequisitesCommand ToCommand(Guid volunteerId) => new()
    {
        Id = volunteerId,
        Requisites = Requisites
    };
}