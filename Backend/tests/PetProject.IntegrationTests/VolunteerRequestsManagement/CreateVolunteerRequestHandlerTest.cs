using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData.Creators;
using VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;
using Random = PetProject.SharedTestData.Creators.Random;


namespace PetProject.IntegrationTests.VolunteerRequestsManagement;

public class CreateVolunteerRequestHandlerTest : VolunteerRequestsManagementTestsBase
{
    private readonly IRequestHandler<CreateVolunteerRequestCommand, Result<Guid, ErrorList>> _sut;

    public CreateVolunteerRequestHandlerTest(VolunteerRequestsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<CreateVolunteerRequestCommand, Result<Guid, ErrorList>>>();
    }

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

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Errors.Should().Contain(e => e.Code == "value.is.invalid");
    }

    [Fact]
    public async Task ShouldCreateVolunteerRequest()
    {
        // Arrange
        var command = Command;

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}