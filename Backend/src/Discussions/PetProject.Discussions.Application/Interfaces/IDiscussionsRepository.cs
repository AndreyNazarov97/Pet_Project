using CSharpFunctionalExtensions;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Application.Interfaces;

public interface IDiscussionsRepository
{
    public Task<Discussion> Add(Discussion discussion,
        CancellationToken cancellationToken = default);

    public Task<Result<Guid, Error>> Delete(Discussion discussion,
        CancellationToken cancellationToken = default);

    public Task<Result<Discussion, Error>> GetByRelationId(Guid relationId,
        CancellationToken cancellationToken = default);
}