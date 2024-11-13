using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber,
    IEnumerable<SocialLinkDto> SocialLinks,
    IEnumerable<RequisiteDto> Requisites);