using PetProject.Application.Dto;

namespace PetProject.Application.VolunteersManagement.UpdateVolunteer;

public record UpdateVolunteerRequest(
    Guid IdVolunteer,
    UpdateVolunteerDto Dto);
    