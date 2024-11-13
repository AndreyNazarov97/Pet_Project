using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.UpdateVolunteer;

public record UpdateVolunteerCommand(
    Guid IdVolunteer,
    UpdateVolunteerDto Dto);
    