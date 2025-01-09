using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.SharedTestData;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.AddPetPhoto;

namespace PetProject.Application.Tests.VolunteerManagement;

public class AddPetPhotoHandlerTest
{
    private static AddPetPhotoCommand AddPetPhotoCommand => new AddPetPhotoCommand
    {
        VolunteerId = Guid.NewGuid(),
        PetId = Guid.NewGuid(),
        Keys = [$"{Guid.NewGuid()}.jpg", $"{Guid.NewGuid()}.jpg"]
    };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerDoesNotExist()
    {
        // Arrange
        var command = AddPetPhotoCommand;

        var handler = StubFactory.CreateAddPetPhotoHandlerStub();

        // Act
        handler.UnitOfWorkMock.SetupTransaction();
        handler.VolunteersRepositoryMock.SetupGetById(
            VolunteerId.Create(command.VolunteerId),
            Errors.General.NotFound(command.VolunteerId));
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(command.VolunteerId));
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(
                    It.IsAny<VolunteerId>(),
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .BeginTransactionAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPetDoesNotExist()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = AddPetPhotoCommand with
        {
            VolunteerId = volunteer.Id.Id,
        };

        var handler = StubFactory.CreateAddPetPhotoHandlerStub();

        // Act
        handler.UnitOfWorkMock.SetupTransaction();
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(command.PetId));
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(
                    It.IsAny<VolunteerId>(),
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .BeginTransactionAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldAddPetPhoto_WhenPetExists()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        var command = AddPetPhotoCommand with
        {
            VolunteerId = volunteer.Id.Id,
            PetId = pet.Id.Id
        };

        var handler = StubFactory.CreateAddPetPhotoHandlerStub();

        // Act
        handler.UnitOfWorkMock.SetupTransaction();
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(
                    It.IsAny<VolunteerId>(),
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .BeginTransactionAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once());

        result.Value.Length.Should().Be(2);
    }
}