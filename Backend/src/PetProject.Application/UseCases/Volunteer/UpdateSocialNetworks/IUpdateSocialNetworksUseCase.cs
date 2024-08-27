using PetProject.Domain.Shared;

namespace PetProject.Application.UseCases.Volunteer.UpdateSocialNetworks;

public interface IUpdateSocialNetworksUseCase
{
    Task<Result> UpdateSocialNetworks(UpdateSocialNetworksRequest request, CancellationToken cancellationToken);
}