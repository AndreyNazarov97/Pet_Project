using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.SoftDeleteVolunteer;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement;
using PetProject.SharedTestData;

namespace PetProject.Application.Tests.VolunteerManagement;

public class SoftDeleteVolunteerHandlerTest
{
    private static SoftDeleteVolunteerCommand Command => new()
    {
        VolunteerId = Guid.NewGuid()
    };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerDoesNotExist()
    {
        // Arrange
        var command = Command;

        var handler = StubFactory.CreateDeleteVolunteerHandlerStub();

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
    public async Task Handle_ShouldDeleteVolunteerSuccessfully_WhenVolunteerExists()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = Command with { VolunteerId = volunteer.Id.Id };

        var handler = StubFactory.CreateDeleteVolunteerHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(
            volunteer.Id,
            volunteer);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(volunteer.Id, It.IsAny<CancellationToken>()),
            Times.Once);

        var isDeletedField = typeof(Volunteer).GetField("_isDeleted",
            BindingFlags.NonPublic | BindingFlags.Instance);
        var isDeleted = (bool)isDeletedField.GetValue(volunteer);
        isDeleted.Should().BeTrue();

        // Проверка логгирования
        handler.LoggerMock.Verify(
            logger => logger.Log(LogLevel.Information, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Volunteer deleted with Id {volunteer.Id}")),
                It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}