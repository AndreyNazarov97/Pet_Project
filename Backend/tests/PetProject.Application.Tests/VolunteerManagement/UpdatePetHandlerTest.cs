using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Core.Database.Models;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.UpdatePet;

namespace PetProject.Application.Tests.VolunteerManagement;

public class UpdatePetHandlerTest
{
    private static UpdatePetCommand Command => new()
    {
        PetId = Guid.NewGuid(),
        VolunteerId = Guid.NewGuid(),
        PetInfo = TestData.PetDto
    };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerDoesNotExist()
    {
        // Arrange
        var command = Command;

        var species = TestData.SpeciesDto;

        var handler = StubFactory.CreateUpdatePetHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(
            VolunteerId.Create(command.VolunteerId),
            Errors.General.NotFound(command.VolunteerId));
        handler.ReadRepositoryMock.SetupQuerySpecies([species]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
        handler.ReadRepositoryMock.Verify(repo => repo.QuerySpecies(
                It.IsAny<SpeciesQueryModel>(), It.IsAny<CancellationToken>()),
            Times.Never);

        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenPetDoesNotExist()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = Command with { VolunteerId = volunteer.Id.Id };

        var species = TestData.SpeciesDto;

        var handler = StubFactory.CreateUpdatePetHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        handler.ReadRepositoryMock.SetupQuerySpecies([species]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
        handler.ReadRepositoryMock.Verify(repo => repo.QuerySpecies(
                It.IsAny<SpeciesQueryModel>(), It.IsAny<CancellationToken>()),
            Times.Once);

        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSpeciesDoesNotExist()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = Command with { VolunteerId = volunteer.Id.Id };

        var handler = StubFactory.CreateUpdatePetHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        handler.ReadRepositoryMock.SetupQueryVolunteer([]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
        handler.ReadRepositoryMock.Verify(repo => repo.QuerySpecies(
                It.IsAny<SpeciesQueryModel>(), It.IsAny<CancellationToken>()),
            Times.Once);

        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenBreedDoesNotExist()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = Command with { VolunteerId = volunteer.Id.Id };

        var species = TestData.SpeciesDto with { Breeds = [] };

        var handler = StubFactory.CreateUpdatePetHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        handler.ReadRepositoryMock.SetupQuerySpecies([species]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
        handler.ReadRepositoryMock.Verify(repo => repo.QuerySpecies(
                It.IsAny<SpeciesQueryModel>(), It.IsAny<CancellationToken>()),
            Times.Once);

        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldUpdatePet_WhenPetExists()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        var command = Command with { VolunteerId = volunteer.Id.Id, PetId = pet.Id.Id };

        var species = TestData.SpeciesDto;

        var handler = StubFactory.CreateUpdatePetHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        handler.ReadRepositoryMock.SetupQuerySpecies([species]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
        handler.ReadRepositoryMock.Verify(repo => repo.QuerySpecies(
                It.IsAny<SpeciesQueryModel>(), It.IsAny<CancellationToken>()),
            Times.Once);

        result.Value.Should().Be(command.PetId);
        pet.PetName.Value.Should().Be(command.PetInfo.PetName);
        pet.AnimalType.BreedName.Value.Should().Be(command.PetInfo.BreedName);
        pet.AnimalType.SpeciesName.Value.Should().Be(command.PetInfo.SpeciesName);
        pet.Address.Country.Should().Be(command.PetInfo.Address!.Country);
        pet.Address.City.Should().Be(command.PetInfo.Address!.City);
        pet.Address.Street.Should().Be(command.PetInfo.Address!.Street);
        pet.Address.House.Should().Be(command.PetInfo.Address!.House);
        pet.Address.Flat.Should().Be(command.PetInfo.Address!.Flat);
        pet.GeneralDescription.Value.Should().Be(command.PetInfo.GeneralDescription);
        pet.HealthInformation.Value.Should().Be(command.PetInfo.HealthInformation);
        pet.IsCastrated.Should().Be((bool)command.PetInfo.IsCastrated!);
        pet.IsVaccinated.Should().Be((bool)command.PetInfo.IsVaccinated!);
    }
}