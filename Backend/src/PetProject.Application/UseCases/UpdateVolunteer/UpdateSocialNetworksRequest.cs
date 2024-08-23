using PetProject.Domain.Dto;

namespace PetProject.Application.UseCases.UpdateVolunteer;

public class UpdateSocialNetworksRequest
{
    public List<SocialNetworkDto> SocialNetworks { get; set; }
}