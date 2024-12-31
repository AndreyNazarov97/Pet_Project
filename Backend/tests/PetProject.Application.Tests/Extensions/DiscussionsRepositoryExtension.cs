using CSharpFunctionalExtensions;
using Moq;
using PetProject.Discussions.Application.Interfaces;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Application.Tests.Extensions;

public static class DiscussionsRepositoryExtension
{
    public static void SetupGetById(
        this Mock<IDiscussionsRepository> mock,
        Result<Discussion, Error> result)
    {
        mock.Setup(x => x.GetById(
            It.IsAny<DiscussionId>(),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(result);
    }
    
    public static void SetupGetByRelationId(
        this Mock<IDiscussionsRepository> mock,
        Result<Discussion, Error> result)
    {
        mock.Setup(x => x.GetByRelationId(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(result);
    }
}