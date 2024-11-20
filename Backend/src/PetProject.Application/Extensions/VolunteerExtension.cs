using PetProject.Application.Dto;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Extensions;

public static class VolunteerExtension
{
    public static FullNameDto ToDto(this FullName fullName)
    {
        return new FullNameDto(fullName.Name, fullName.Surname, fullName.Patronymic);
    }

    public static RequisiteDto ToDto(this Requisite requisite)
    {
        return new RequisiteDto(requisite.Title, requisite.Description);
    }
    
    public static RequisiteDto[] ToDto(this RequisitesList requisitesList)
    {
        return requisitesList.Requisites.Select(x => x.ToDto()).ToArray();
    }
    public static SocialLinkDto ToDto(this SocialLink socialLink)
    {
        return new SocialLinkDto(socialLink.Title, socialLink.Url);
    }
    
    public static SocialLinkDto[] ToDto(this SocialLinksList socialLinksList)
    {
        return socialLinksList.SocialLinks.Select(x => x.ToDto()).ToArray();
    }
}