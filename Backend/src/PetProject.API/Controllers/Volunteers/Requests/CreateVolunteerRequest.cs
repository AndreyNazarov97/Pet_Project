using PetProject.Application.Dto;
using PetProject.Application.Volunteers.CreateVolunteer;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber,
    IEnumerable<SocialLinkDto> SocialLinks,
    IEnumerable<RequisiteDto> Requisites)
{
    public CreateVolunteerCommand ToCommand() => new()
    {
        FullName = FullName,
        Description = Description,
        AgeExperience = AgeExperience,
        PhoneNumber = PhoneNumber,
        SocialLinks = SocialLinks,
        Requisites = Requisites
    };
}