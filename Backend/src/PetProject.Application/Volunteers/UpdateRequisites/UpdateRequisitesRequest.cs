using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.UpdateRequisites;

public record UpdateRequisitesRequest(
    Guid Id, 
    UpdateRequisitesDto Dto);