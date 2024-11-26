using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.ChangePetStatus;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.SharedTestData;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Application.Tests.VolunteerManagement;

public class ChangePetStatusHandlerStubTest
{
    private static ChangePetStatusCommand Command => new()
    {
        VolunteerId = Guid.NewGuid(),
        PetId = Guid.NewGuid(),
        HelpStatus = Random.HelpStatus
    };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        //Arrange
        var command = Command;
        var handler = StubFactory.CreateChangePetStatusHandlerStub();
        
        //Act
        handler.VolunteersRepositoryMock.SetupGetById(
            VolunteerId.Create(command.VolunteerId),
            Errors.General.NotFound(command.VolunteerId));
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(
                    It.IsAny<VolunteerId>(), 
                    It.IsAny<CancellationToken>()),
            Times.Once);
        
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPetNotFound()
    {
        //Arrange
        var volunteer = TestData.Volunteer;
        var command = Command with{VolunteerId = volunteer.Id.Id};
        var handler = StubFactory.CreateChangePetStatusHandlerStub();
        
        //Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(
                    It.IsAny<VolunteerId>(), 
                    It.IsAny<CancellationToken>()),
            Times.Once);
        
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenStatusChanged()
    {
        //Arrange
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        var command = Command with{VolunteerId = volunteer.Id.Id, PetId = pet.Id.Id};
        var handler = StubFactory.CreateChangePetStatusHandlerStub();
        
        //Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        pet.HelpStatus.Should().Be(command.HelpStatus);
        
        handler.VolunteersRepositoryMock.Verify(repo => repo
            .GetById(
            It.IsAny<VolunteerId>(), 
            It.IsAny<CancellationToken>()),
            Times.Once);
        
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}