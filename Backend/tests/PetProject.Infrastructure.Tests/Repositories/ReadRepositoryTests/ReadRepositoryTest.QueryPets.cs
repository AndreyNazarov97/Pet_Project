using FluentAssertions;
using PetProject.Core.Database.Models;
using PetProject.SharedTestData;

namespace PetProject.Infrastructure.Tests.Repositories.ReadRepositoryTests;

public partial class ReadRepositoryTest
{
    [Fact]
    public async Task ShouldReturnAllPets_WhenNoFiltersAreApplied()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet1 = TestData.Pet;
        volunteer.AddPet(pet1);
        var volunteer2 = TestData.Volunteer;
        var pet2 = TestData.Pet;
        volunteer2.AddPet(pet2);
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.AddAsync(volunteer2);
        dbContext.SaveChanges();

        var query = new PetQueryModel();

        // Act
        var result = await _sut.QueryPets(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.PetName == pet1.PetName.Value);
        result.Should().Contain(p => p.PetName == pet2.PetName.Value);
    }

    [Fact]
    public async Task ShouldReturnPetById()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.SaveChangesAsync();

        var query = new PetQueryModel()
        {
            PetId = pet.Id.Id
        };

        // Act
        var result = await _sut.QueryPets(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().PetName.Should().Be(pet.PetName.Value);
        result.First().BreedName.Should().Be(pet.AnimalType.BreedName.Value);
        result.First().SpeciesName.Should().Be(pet.AnimalType.SpeciesName.Value);
        result.First().PhoneNumber.Should().Be(volunteer.PhoneNumber.Value);
        result.First().GeneralDescription.Should().Be(pet.GeneralDescription.Value);
        result.First().HealthInformation.Should().Be(pet.HealthInformation.Value);
        result.First().Weight.Should().Be(pet.PhysicalAttributes.Weight);
        result.First().Height.Should().Be(pet.PhysicalAttributes.Height);
        result.First().BirthDate.Should().NotBeNull();
        result.First().IsCastrated.Should().Be(pet.IsCastrated);
        result.First().IsVaccinated.Should().Be(pet.IsVaccinated);
        result.First().HelpStatus.Should().Be(pet.HelpStatus.ToString());
        result.First().Address!.Country.Should().Be(pet.Address.Country);
        result.First().Address!.City.Should().Be(pet.Address.City);
        result.First().Address!.Street.Should().Be(pet.Address.Street);
        result.First().Address!.House.Should().Be(pet.Address.House);
        result.First().Address!.Flat.Should().Be(pet.Address.Flat);
    }

    [Fact]
    public async Task Should_ReturnEmptyList_WhenPetNotFound()
    {
        var query = new PetQueryModel() { PetId = Guid.NewGuid() };

        // Act
        var result = await _sut.QueryPets(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task ShouldFilterPetsByName()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet1 = TestData.Pet;
        volunteer.AddPet(pet1);
        var volunteer2 = TestData.Volunteer;
        var pet2 = TestData.Pet;
        volunteer2.AddPet(pet2);
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.AddAsync(volunteer2);
        await dbContext.SaveChangesAsync();

        var query = new PetQueryModel
        {
            Name = pet1.PetName.Value
        };

        // Act
        var result = await _sut.QueryPets(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().PetName.Should().Be(pet1.PetName.Value);
    }

    [Fact]
    public async Task ShouldFilterPetsByMinAge()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.SaveChangesAsync();

        var minAge = (DateTime.Now.Year - pet.BirthDate.Year) + 1;
        var query = new PetQueryModel
        {
            MinAge = minAge
        };

        // Act
        var result = await _sut.QueryPets(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task ShouldSortPetsByName()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet1 = TestData.Pet;
        volunteer.AddPet(pet1);
        var volunteer2 = TestData.Volunteer;
        var pet2 = TestData.Pet;
        volunteer2.AddPet(pet2);
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.AddAsync(volunteer2);
        await dbContext.SaveChangesAsync();

        var query = new PetQueryModel
        {
            SortBy = "Name",
            SortDescending = false
        };

        var petsInDb = _fixture.GetVolunteerDbContext().Volunteers
            .SelectMany(v => v.Pets)
            .Select(p => p.PetName.Value)
            .OrderBy(p => p)
            .ToList();

        // Act
        var result = await _sut.QueryPets(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result[0].PetName.Should().Be(petsInDb[0]);
        result[1].PetName.Should().Be(petsInDb[1]);
    }

    [Fact]
    public async Task ShouldFilterPetsBySpeciesAndBreed()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet1 = TestData.Pet;
        volunteer.AddPet(pet1);
        var volunteer2 = TestData.Volunteer;
        var pet2 = TestData.Pet;
        volunteer2.AddPet(pet2);
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.AddAsync(volunteer2);
        await dbContext.SaveChangesAsync();

        var query = new PetQueryModel
        {
            Species = pet1.AnimalType.SpeciesName.Value,
            Breed = pet1.AnimalType.BreedName.Value
        };

        // Act
        var result = await _sut.QueryPets(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().PetName.Should().Be(pet1.PetName.Value);
    }
}