using PetProject.Core.Dtos;
using PetProject.VolunteerManagement.Application.VolunteersManagement.UpdateSocialLinks;

namespace PetProject.VolunteerManagement.Presentation.Requests;

public record UpdateSocialLinksRequest(IEnumerable<SocialNetworkDto> SocialLinks)
{
    public UpdateSocialLinksCommand ToCommand(Guid volunteerId) => new()
    {
        VolunteerId = volunteerId,
        SocialLinks = SocialLinks
    };
}