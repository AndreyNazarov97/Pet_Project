using System.Reflection;
using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SoftDeletePet;
using PetProject.VolunteerManagement.Domain.Entities;

namespace PetProject.Application.Tests.VolunteerManagement;

public class SoftDeletePetHandlerTest
{
    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        // Arrange
        var command = new SoftDeletePetCommand {PetId = Guid.NewGuid(), VolunteerId = Guid.NewGuid()};
        var handler = StubFactory.CreateSoftDeletePetHandlerStub();
        
        // Act
        handler.VolunteersRepositoryMock.SetupGetById(
            VolunteerId.Create(command.VolunteerId),
            Errors.General.NotFound(command.VolunteerId));
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);

        handler.UnitOfWorkMock.Verify(u => u
                .SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenPetNotFound()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = new SoftDeletePetCommand {PetId = Guid.NewGuid(), VolunteerId = volunteer.Id.Id};
        var handler = StubFactory.CreateSoftDeletePetHandlerStub();
        
        // Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);

        handler.UnitOfWorkMock.Verify(u => u
                .SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldSoftDeletePet()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        var command = new SoftDeletePetCommand(){PetId = pet.Id.Id, VolunteerId = volunteer.Id.Id};
        var handler = StubFactory.CreateSoftDeletePetHandlerStub();
        
        // Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo
            .GetById(It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);

        handler.UnitOfWorkMock.Verify(u => u
            .SaveChangesAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
        
        pet.IsDeleted.Should().BeTrue();
    }
}