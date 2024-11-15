using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.UpdateSocialLinks;

public record UpdateSocialLinksCommand()
{
    public required Guid Id { get; init; } 
    public required IEnumerable<SocialLinkDto> SocialLinks { get; init; } 
}