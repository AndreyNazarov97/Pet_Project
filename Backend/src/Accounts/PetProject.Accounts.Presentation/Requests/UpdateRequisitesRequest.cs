using PetProject.Accounts.Application.AccountManagement.Commands.UpdateRequisites;
using PetProject.Core.Dtos;

namespace PetProject.Accounts.Presentation.Requests;

public record UpdateRequisitesRequest(IEnumerable<RequisiteDto> Requisites)
{
    public UpdateRequisitesCommand ToCommand(long userId) => new()
    {
        UserId = userId,
        Requisites = Requisites
    };
}