using FluentAssertions;
using Moq;
using PetProject.Application.Models;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.CreatePet;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.SharedTestData;
using PetProject.SharedTestData.Creators;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Application.Tests.VolunteerManagement;

public class CreatePetHandlerTest
{
    private static CreatePetCommand CreatePetCommand => new()
    {
        VolunteerId = Guid.NewGuid(),
        Name = Random.Name,
        GeneralDescription = Random.LoremParagraph,
        HealthInformation = Random.LoremParagraph,
        SpeciesName = "Dog",
        BreedName = "Bulldog",
        Address = VolunteerCreator.CreateAddressDto(),
        Weight = Random.Double,
        Height = Random.Double,
        BirthDate = Random.DateOnly,
        IsCastrated = Random.Bool,
        IsVaccinated = Random.Bool,
        HelpStatus = Random.HelpStatus
    };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        // Arrange
        var command = CreatePetCommand;
        var handler = StubFactory.CreateCreatePetHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(
            VolunteerId.Create(command.VolunteerId),
            Errors.General.NotFound(command.VolunteerId));
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSpeciesNotFound()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = CreatePetCommand with { VolunteerId = volunteer.Id };
        
        var handler = StubFactory.CreateCreatePetHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        handler.SpeciesRepositoryMock.SetupQuery([]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.SpeciesRepositoryMock.Verify(repo => repo.Query(
                It.IsAny<SpeciesQueryModel>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenBreedNotFoundInSpecies()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = CreatePetCommand with { VolunteerId = volunteer.Id, BreedName = "WrongBreedName" };

        var speciesDto = TestData.SpeciesDto;
        
        var handler = StubFactory.CreateCreatePetHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        handler.SpeciesRepositoryMock.SetupQuery([speciesDto]);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        handler.SpeciesRepositoryMock.Verify(repo => repo.Query(
                It.IsAny<SpeciesQueryModel>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreatePet_WhenPetDoesNotExist()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = CreatePetCommand with { VolunteerId = volunteer.Id };

        var speciesDto = TestData.SpeciesDto;
        
        var handler = StubFactory.CreateCreatePetHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        handler.SpeciesRepositoryMock.SetupQuery([speciesDto]);
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
    }
}