using PetProject.Domain.Dto;

namespace PetProject.Application.UseCases.Volunteer.UpdateRequisites;

public record UpdateRequisitesRequest(
    Guid VolunteerId,
    UpdateRequisitesDto Dto
);