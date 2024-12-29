using CSharpFunctionalExtensions;
using Moq;
using PetProject.Discussions.Contracts;
using PetProject.Discussions.Contracts.Requests;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.SharedKernel.Shared;

namespace PetProject.Application.Tests.Extensions;

public static class DiscussionContractExtension
{
    public static void SetupCreate(this Mock<IDiscussionContract> mock, Result<Discussion, ErrorList> result)
    {
        mock.Setup(x => x.Create(
                It.IsAny<CreateDiscussionRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
    }
}