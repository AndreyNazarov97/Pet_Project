using PetProject.Application.Dto;
using PetProject.Application.Volunteers.UpdateSocialLinks;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record UpdateSocialLinksRequest(IEnumerable<SocialLinkDto> SocialLinks)
{
    public UpdateSocialLinksCommand ToCommand(Guid volunteerId) => new()
    {
        Id = volunteerId,
        SocialLinks = SocialLinks
    };
}