using CSharpFunctionalExtensions;
using Moq;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Domain.Aggregate;

namespace PetProject.Application.Tests.Extensions;

public static class VolunteersRequestsRepositoryExtensions
{
    public static void SetupGetById(
        this Mock<IRequestsRepository> mock,
        Result<VolunteerRequest, Error> result)
    {
        mock.Setup(vr => vr.GetById(
                It.IsAny<VolunteerRequestId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(result);
    }
}