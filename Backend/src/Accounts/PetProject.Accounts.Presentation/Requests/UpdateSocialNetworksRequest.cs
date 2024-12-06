using PetProject.Accounts.Application.AccountManagement.Commands.UpdateSocialNetworks;
using PetProject.Core.Dtos;

namespace PetProject.Accounts.Presentation.Requests;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialLinks)
{
    public UpdateSocialNetworksCommand ToCommand(long userId) => new()
    {
        UserId = userId,
        SocialLinks = SocialLinks
    };
}