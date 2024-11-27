using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Domain.Aggregate;

namespace PetProject.VolunteerManagement.Application.Extensions;

public static class VolunteerExtension
{
    public static VolunteerDto ToDto(this Volunteer volunteer) => new()
    {
        FullName = volunteer.FullName.ToDto(),
        GeneralDescription = volunteer.GeneralDescription.Value,
        AgeExperience = volunteer.Experience.Years,
        PhoneNumber = volunteer.PhoneNumber.Value,
        Requisites = volunteer.Requisites.Select(x => x.ToDto()).ToArray(),
        SocialLinks = volunteer.SocialLinks.Select(x => x.ToDto()).ToArray()
    };
    
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
        return new RequisiteDto
        {
            Title = requisite.Title,
            Description = requisite.Description
        };
    }
    public static SocialLinkDto ToDto(this SocialLink socialLink)
    {
        return new SocialLinkDto
        {
            Title = socialLink.Title,
            Url = socialLink.Url
        };
    }
}