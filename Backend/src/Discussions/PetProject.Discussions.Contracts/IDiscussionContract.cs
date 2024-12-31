using CSharpFunctionalExtensions;
using PetProject.Discussions.Contracts.Requests;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Contracts;

public interface IDiscussionContract
{
    Task<Result<Discussion, ErrorList>> Create(
        CreateDiscussionRequest request, CancellationToken cancellationToken = default);
}