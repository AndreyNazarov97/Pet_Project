using PetProject.Core.Dtos;

namespace PetProject.Accounts.Contracts.Requests;

public record UpdateRequisitesRequest(IEnumerable<RequisiteDto> Requisites);