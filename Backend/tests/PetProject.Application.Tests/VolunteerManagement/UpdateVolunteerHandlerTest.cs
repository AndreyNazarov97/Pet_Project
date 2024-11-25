using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.UpdateVolunteer;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.SharedTestData;
using PetProject.SharedTestData.Creators;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Application.Tests.VolunteerManagement;

public class UpdateVolunteerHandlerTest
{
    private static UpdateVolunteerCommand UpdateVolunteerCommand => new()
    {
        VolunteerId = Guid.NewGuid(),
        FullName = VolunteerCreator.CreateFullNameDto(),
        Description = Random.LoremParagraph,
        AgeExperience = Random.Experience,
        PhoneNumber = Random.PhoneNumber
    };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        // Arrange
        var command = UpdateVolunteerCommand;
        var handler = StubFactory.CreateUpdateVolunteerHandlerStub();
        // Act
        handler.VolunteersRepositoryMock.SetupGetById(
            VolunteerId.Create(command.VolunteerId),
            Errors.General.NotFound(command.VolunteerId));

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdateVolunteer_WhenVolunteerExists()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = UpdateVolunteerCommand with { VolunteerId = volunteer.Id.Id };

        var handler = StubFactory.CreateUpdateVolunteerHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(
            volunteer.Id,
            volunteer);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.UnitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        volunteer.FullName.Name.Should().Be(command.FullName.Name);
        volunteer.FullName.Surname.Should().Be(command.FullName.Surname);
        volunteer.FullName.Patronymic.Should().Be(command.FullName.Patronymic);
        volunteer.GeneralDescription.Value.Should().Be(command.Description);
        volunteer.Experience.Years.Should().Be(command.AgeExperience);
        volunteer.PhoneNumber.Value.Should().Be(command.PhoneNumber);
    }
}