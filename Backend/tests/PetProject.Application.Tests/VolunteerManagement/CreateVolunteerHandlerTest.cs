using FluentAssertions;
using Moq;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.CreateVolunteer;
using PetProject.SharedTestData;
using PetProject.SharedTestData.Creators;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Application.Tests.VolunteerManagement;

public class CreateVolunteerHandlerTest
{
    private static CreateVolunteerCommand Command => new ()
    {
        PhoneNumber = Random.PhoneNumber,
        FullName = VolunteerCreator.CreateFullNameDto(),
        Description = Random.LoremParagraph,
        AgeExperience = Random.Experience,
        SocialLinks = new List<SocialLinkDto>(),
        Requisites = new List<RequisiteDto>()
    };
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerAlreadyExists()
    {
        // Arrange
        var existingVolunteer = TestData.Volunteer;
        var command = Command with { PhoneNumber = existingVolunteer.PhoneNumber.Value };

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
        var command = Command;

        var handler = StubFactory.CreateVolunteerHandlerStub();
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.Query(
            It.IsAny<VolunteerQueryModel>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}