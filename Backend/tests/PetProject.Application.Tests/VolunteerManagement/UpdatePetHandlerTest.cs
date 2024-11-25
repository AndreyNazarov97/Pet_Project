﻿using FluentAssertions;
using Moq;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.UpdatePet;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.SharedTestData;

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
        handler.SpeciesRepositoryMock.SetupQuery([species]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
        handler.SpeciesRepositoryMock.Verify(repo => repo.Query(
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
        handler.SpeciesRepositoryMock.SetupQuery([species]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
        handler.SpeciesRepositoryMock.Verify(repo => repo.Query(
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
        handler.SpeciesRepositoryMock.SetupQuery([]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
        handler.SpeciesRepositoryMock.Verify(repo => repo.Query(
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
        handler.SpeciesRepositoryMock.SetupQuery([species]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
        handler.SpeciesRepositoryMock.Verify(repo => repo.Query(
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
        handler.SpeciesRepositoryMock.SetupQuery([species]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
        handler.SpeciesRepositoryMock.Verify(repo => repo.Query(
                It.IsAny<SpeciesQueryModel>(), It.IsAny<CancellationToken>()),
            Times.Once);

        result.Value.Should().Be(command.PetId);
    }
}