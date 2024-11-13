using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.UpdateRequisites;

public record UpdateRequisitesCommand(
    Guid Id, 
    UpdateRequisitesDto Dto);