using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Domain.Aggregate;

namespace VolunteerRequests.Application.Repositories;

public interface IRequestsRepository
{
    public Task<Guid> Add(VolunteerRequest request,
        CancellationToken cancellationToken = default);

    public Task<Result<Guid, Error>> Delete(VolunteerRequest request,
        CancellationToken cancellationToken = default);

    public Task<Result<VolunteerRequest, Error>> GetById(VolunteerRequestId id,
        CancellationToken cancellationToken = default);
}