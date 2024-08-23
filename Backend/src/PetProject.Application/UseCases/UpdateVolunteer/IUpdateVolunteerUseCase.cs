using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.UseCases.UpdateVolunteer;

public interface IUpdateVolunteerUseCase
{
    Task<Result> UpdateMainInfo(VolunteerId id, UpdateMainInfoRequest request, CancellationToken cancellationToken);
    Task<Result> UpdateSocialNetworks(VolunteerId id, UpdateSocialNetworksRequest request, CancellationToken cancellationToken);
    Task<Result> UpdateRequisites(VolunteerId id, UpdateRequisitesRequest request, CancellationToken cancellationToken);
}