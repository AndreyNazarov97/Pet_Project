using PetProject.Core.Dtos;

namespace PetProject.Accounts.Contracts.Requests;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialLinks);