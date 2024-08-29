using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.UpdateVolunteer;

public record UpdateVolunteerRequest(
    Guid IdVolunteer,
    UpdateVolunteerDto Dto);
    