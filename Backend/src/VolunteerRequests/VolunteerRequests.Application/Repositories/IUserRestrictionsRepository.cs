using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Domain.Aggregate;

namespace VolunteerRequests.Application.Repositories;

public interface IUserRestrictionsRepository
{
    public Task<Guid> Add(UserRestriction restriction,
        CancellationToken cancellationToken = default);

    public Task<Result<Guid, Error>> Delete(UserRestriction restriction,
        CancellationToken cancellationToken = default);

    public Task<Result<UserRestriction, Error>> GetById(UserRestrictionId id,
        CancellationToken cancellationToken = default);
    
    public Task<Result<UserRestriction, Error>> GetByUserId(long userId,
        CancellationToken cancellationToken = default);
}