using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Domain.Aggregate;
using PetProject.VolunteerManagement.Domain.ValueObjects;

namespace PetProject.VolunteerManagement.Application.Extensions;

public static class VolunteerExtension
{
    public static VolunteerDto ToDto(this Volunteer volunteer) => new()
    {
        FullName = volunteer.FullName.ToDto(),
        GeneralDescription = volunteer.GeneralDescription.Value,
        AgeExperience = volunteer.Experience.Years,
        PhoneNumber = volunteer.PhoneNumber.Value
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

    public static PhotoDto[] ToDto(this List<PetPhoto> petPhotos)
    {
        var photos = petPhotos
            .Select(p => new PhotoDto
            {
                FileId = p.FileId,
                IsMain = p.IsMain
            });
        return photos.ToArray();
    }

    public static RequisiteDto ToDto(this Requisite requisite)
    {
        return new RequisiteDto
        {
            Title = requisite.Title,
            Description = requisite.Description
        };
    }
    public static SocialNetworkDto ToDto(this SocialNetwork socialNetwork)
    {
        return new SocialNetworkDto
        {
            Title = socialNetwork.Title,
            Url = socialNetwork.Url
        };
    }
}