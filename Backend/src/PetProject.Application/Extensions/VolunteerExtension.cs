using PetProject.Application.Dto;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Extensions;

public static class VolunteerExtension
{
    public static FullNameDto ToDto(this FullName fullName)
    {
        return new FullNameDto()
        {
            Name = fullName.Name,
            Surname = fullName.Surname,
            Patronymic = fullName.Patronymic
        };
    }

    public static RequisiteDto ToDto(this Requisite requisite)
    {
        return new RequisiteDto(requisite.Title, requisite.Description);
    }
    public static SocialLinkDto ToDto(this SocialLink socialLink)
    {
        return new SocialLinkDto(socialLink.Title, socialLink.Url);
    }
}