using PetProject.Accounts.Application.AccountManagement.Commands.UpdateSocialLinks;
using PetProject.Core.Dtos;

namespace PetProject.Accounts.Presentation.Requests;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialLinks)
{
    public UpdateSocialLinksCommand ToCommand(long userId) => new()
    {
        UserId = userId,
        SocialLinks = SocialLinks
    };
}