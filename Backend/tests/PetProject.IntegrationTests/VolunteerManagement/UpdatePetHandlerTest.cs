using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.UpdatePet;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class UpdatePetHandlerTest : VolunteerManagementTestsBase
{
    private readonly IRequestHandler<UpdatePetCommand, Result<Guid, ErrorList>> _sut;

    public UpdatePetHandlerTest(BaseTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<UpdatePetCommand, Result<Guid, ErrorList>>>();
    }

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

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenPetDoesNotExist()
    {
        // Arrange
        var volunteer = await SeedVolunteer();
        var command = Command with { VolunteerId = volunteer.Id.Id };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSpeciesDoesNotExist()
    {
        // Arrange
        var volunteer = await SeedVolunteer();
        var command = Command with { VolunteerId = volunteer.Id.Id };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenBreedDoesNotExist()
    {
        // Arrange
        var volunteer = await SeedVolunteer();
        var command = Command with { VolunteerId = volunteer.Id.Id };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldUpdatePet_WhenPetExists()
    {
        // Arrange
        await SeedSpecies();
        var volunteer = await SeedVolunteer();
        var command = Command with
        {
            VolunteerId = volunteer.Id.Id, 
            PetId = volunteer.Pets[0].Id.Id
        };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var volunteerFromDb = await _volunteerDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteer.Id, CancellationToken.None);
        
        var pet = volunteerFromDb!.Pets.First();

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