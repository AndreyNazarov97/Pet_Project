using PetProject.Domain.Dto;

namespace PetProject.Application.UseCases.Volunteer.UpdateSocialNetworks;

public record UpdateSocialNetworksRequest(
    Guid VolunteerId,
    UpdateSocialNetworksDto Dto
);