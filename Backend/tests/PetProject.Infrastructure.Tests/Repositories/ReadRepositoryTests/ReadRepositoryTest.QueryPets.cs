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
        await dbContext.SaveChangesAsync();

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
    public async Task ShouldReturnPaginatedResults()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet1 = TestData.Pet;
        volunteer.AddPet(pet1);
        var volunteer2 = TestData.Volunteer;
        var pet2 = TestData.Pet;
        var pet3 = TestData.Pet;
        volunteer2.AddPet(pet2);
        volunteer2.AddPet(pet3);
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.AddAsync(volunteer2);
        await dbContext.SaveChangesAsync();

        var query = new PetQueryModel
        {
            Limit = 2,
            Offset = 1,
            SortBy = "Name",
        };
        
        var petsInDb = _fixture.GetVolunteerDbContext().Volunteers
            .SelectMany(v => v.Pets)
            .Select(p => p.PetName.Value)
            .OrderBy(n => n)
            .ToList();

        // Act
        var result = await _sut.QueryPets(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result[0].PetName.Should().Be(petsInDb[1]);
        result[1].PetName.Should().Be(petsInDb[2]);
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