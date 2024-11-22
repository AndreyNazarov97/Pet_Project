using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Dto;

public record SocialLinkDto(string Title, string Url)
{
    public SocialLink ToEntity()
    {
        var socialLink = SocialLink.Create(Title, Url).Value;
        
        return socialLink;
    }
}