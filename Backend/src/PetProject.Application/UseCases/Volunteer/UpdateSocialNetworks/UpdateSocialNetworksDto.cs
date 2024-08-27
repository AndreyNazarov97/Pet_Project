using PetProject.Domain.Dto;

namespace PetProject.Application.UseCases.Volunteer.UpdateSocialNetworks;

public record UpdateSocialNetworksDto(
    List<SocialNetworkDto> SocialNetworks);