using PetProject.Core.Dtos;
using PetProject.VolunteerManagement.Application.VolunteersManagement.CreateVolunteer;

namespace PetProject.VolunteerManagement.Presentation.Requests;

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