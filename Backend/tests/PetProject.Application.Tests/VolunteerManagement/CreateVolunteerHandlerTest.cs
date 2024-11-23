using FluentAssertions;
using Moq;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Application.Tests.Creators;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.CreateVolunteer;

namespace PetProject.Application.Tests.VolunteerManagement;

public class CreateVolunteerHandlerTest
{
    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerAlreadyExists()
    {
        // Arrange
        var existingVolunteer = TestData.Volunteer;
        var command = new CreateVolunteerCommand
        {
            PhoneNumber = existingVolunteer.PhoneNumber.Value,
            FullName = VolunteerCreator.CreateFullNameDto(),
            Description = "Volunteer description",
            AgeExperience = 5,
            SocialLinks = new List<SocialLinkDto>(),
            Requisites = new List<RequisiteDto>()
        };

        VolunteerDto[] dtos = [TestData.VolunteerDto];

        var handler = StubFactory.CreateVolunteerHandlerStub();
        
        // Act
        handler.VolunteersRepositoryMock.SetupQuery(dtos);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Count.Should().Be(1);
        var error = result.Error.Errors.Single();
        error.Message.Should().Be("Volunteer already exist");
        error.Code.Should().Be("volunteer.already.exist");
    }

    [Fact]
    public async Task ShouldCreateVolunteer()
    {
        var command = new CreateVolunteerCommand
        {
            FullName = VolunteerCreator.CreateFullNameDto(),
            Description = "Test description",
            AgeExperience = 3,
            PhoneNumber = "7234567890",
            SocialLinks = [],
            Requisites = []
        };

        var handler = StubFactory.CreateVolunteerHandlerStub();
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.Query(
            It.IsAny<VolunteerQueryModel>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}