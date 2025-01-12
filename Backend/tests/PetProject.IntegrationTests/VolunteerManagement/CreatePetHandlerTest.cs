using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData.Creators;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.CreatePet;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class CreatePetHandlerTest : VolunteerManagementTestsBase
{
    private readonly IRequestHandler<CreatePetCommand, Result<Guid, ErrorList>> _sut;

    public CreatePetHandlerTest(VolunteerManagementTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<CreatePetCommand, Result<Guid, ErrorList>>>();
        
    }

    private static CreatePetCommand CreatePetCommand => new()
    {
        VolunteerId = Guid.NewGuid(),
        Name = Random.Name,
        GeneralDescription = Random.Words,
        HealthInformation = Random.Words,
        SpeciesName = "Dog",
        BreedName = "Bulldog",
        Address = DtoCreator.CreateAddressDto(),
        Weight = Random.Double,
        Height = Random.Double,
        BirthDate = Random.DateTimeOffset,
        IsCastrated = Random.Bool,
        IsVaccinated = Random.Bool,
        HelpStatus = Random.HelpStatus
    };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        // Arrange
        var command = CreatePetCommand;

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSpeciesNotFound()
    {
        // Arrange
        var volunteer = await SeedVolunteer();
        var command = CreatePetCommand with { VolunteerId = volunteer.Id };

        // Act
        var result = await  _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenBreedNotFoundInSpecies()
    {
        // Arrange
        var volunteer = await SeedVolunteer();
        var command = CreatePetCommand with { 
            VolunteerId = volunteer.Id, 
            BreedName = "WrongBreedName" };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldCreatePet_WhenPetDoesNotExist()
    {
        // Arrange
        await SeedSpecies();
        var volunteer = await SeedVolunteer();
        var command = CreatePetCommand with { VolunteerId = volunteer.Id };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var volunteerFromDb = await _volunteerDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteer.Id, CancellationToken.None);
        var petFromDb = volunteerFromDb!.Pets.First(x => x.PetName.Value == command.Name);

        petFromDb.PetName.Value.Should().Be(command.Name);
        petFromDb.GeneralDescription.Value.Should().Be(command.GeneralDescription);
        petFromDb.HealthInformation.Value.Should().Be(command.HealthInformation);
        petFromDb.AnimalType.SpeciesName.Value.Should().Be(command.SpeciesName);
        petFromDb.AnimalType.BreedName.Value.Should().Be(command.BreedName);
        petFromDb.Address.Country.Should().Be(command.Address.Country);
        petFromDb.Address.City.Should().Be(command.Address.City);
        petFromDb.Address.Street.Should().Be(command.Address.Street);
        petFromDb.Address.House.Should().Be(command.Address.House);
        petFromDb.Address.Flat.Should().Be(command.Address.Flat);
        petFromDb.PhysicalAttributes.Weight.Should().Be(command.Weight);
        petFromDb.PhysicalAttributes.Height.Should().Be(command.Height);
        petFromDb.BirthDate.Should().Be(command.BirthDate);
        petFromDb.IsCastrated.Should().Be(command.IsCastrated);
        petFromDb.IsVaccinated.Should().Be(command.IsVaccinated);
        petFromDb.HelpStatus.Should().Be(command.HelpStatus); 
    }
}