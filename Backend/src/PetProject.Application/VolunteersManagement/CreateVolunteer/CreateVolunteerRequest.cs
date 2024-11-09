using PetProject.Application.Dto;

namespace PetProject.Application.VolunteersManagement.CreateVolunteer;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string Number,
    IEnumerable<SocialLinkDto> SocialLinks,
    IEnumerable<RequisiteDto> Requisites);