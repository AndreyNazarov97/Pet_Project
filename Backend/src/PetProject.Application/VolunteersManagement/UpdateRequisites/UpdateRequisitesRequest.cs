using PetProject.Application.Dto;

namespace PetProject.Application.VolunteersManagement.UpdateRequisites;

public record UpdateRequisitesRequest(
    Guid Id, 
    UpdateRequisitesDto Dto);