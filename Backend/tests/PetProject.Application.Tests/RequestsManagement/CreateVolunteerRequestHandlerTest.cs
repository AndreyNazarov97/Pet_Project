using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Stubs;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData.Creators;
using VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;
using VolunteerRequests.Domain.Aggregate;
using Random = PetProject.SharedTestData.Creators.Random;


namespace PetProject.Application.Tests.RequestsManagement;

public class CreateVolunteerRequestHandlerTest
{
    private static CreateVolunteerRequestCommand Command => new()
    {
        UserId = Random.Long,
        FullName = DtoCreator.CreateFullNameDto(),
        Description = Random.Words,
        AgeExperience = Random.Experience,
        PhoneNumber = Random.PhoneNumber,
        SocialNetworksDto = [DtoCreator.CreateSocialNetworkDto(), DtoCreator.CreateSocialNetworkDto()]
    };

    [Fact]
    public async Task ShouldReturnError_WhenCommandIsInvalid()
    {
        // Arrange
        var command = Command with { UserId = 0 };
        var handler = StubFactory.CreateCreateVolunteerRequestHandlerStub();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Errors.Should().Contain(e => e.Code == "value.is.invalid");
        handler.VolunteerRequestsRepositoryMock.Verify(repo => repo.Add(
                It.IsAny<VolunteerRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ShouldCreateVolunteerRequest()
    {
        // Arrange
        var command = Command;
        var handler = StubFactory.CreateCreateVolunteerRequestHandlerStub();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.VolunteerRequestsRepositoryMock.Verify(repo => repo.Add(
                It.IsAny<VolunteerRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}