using FluentAssertions;
using PetProject.Core.Database.Models;
using PetProject.SharedTestData;
using PetProject.VolunteerManagement.Domain.Aggregate;

namespace PetProject.Infrastructure.Tests.Repositories.ReadRepositoryTests;

public partial class ReadRepositoryTest
{
    [Fact]
    public async Task ShouldReturnVolunteersWithGivenPetIds()
    {
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.SaveChangesAsync();

        var query = new VolunteerQueryModel
        {
            PetIds = [pet.Id.Id]
        };

        var result = await _sut.QueryVolunteers(query, CancellationToken.None);

        result.Should().HaveCount(1);
        result.First().Pets.Should().ContainSingle(p => p.PetName == pet.PetName.Value);
        result.First().Pets.Should().ContainSingle(p => p.BreedName == pet.AnimalType.BreedName.Value);
        result.First().Pets.Should().ContainSingle(p => p.SpeciesName == pet.AnimalType.SpeciesName.Value);
        result.First().Pets.Should().ContainSingle(p => p.PhoneNumber == volunteer.PhoneNumber.Value);
        result.First().Pets.Should().ContainSingle(p => p.GeneralDescription == pet.GeneralDescription.Value);
        result.First().Pets.Should().ContainSingle(p => p.HealthInformation == pet.HealthInformation.Value);
        result.First().Pets.Should().ContainSingle(p => p.Weight == pet.PhysicalAttributes.Weight);
        result.First().Pets.Should().ContainSingle(p => p.Height == pet.PhysicalAttributes.Height);
        result.First().Pets.Should().ContainSingle(p => p.BirthDate != null);
        result.First().Pets.Should().ContainSingle(p => p.IsCastrated == pet.IsCastrated);
        result.First().Pets.Should().ContainSingle(p => p.IsVaccinated == pet.IsVaccinated);
        result.First().Pets.Should().ContainSingle(p => p.HelpStatus == pet.HelpStatus.ToString());
        result.First().Pets.Should().ContainSingle(p => p.Address!.Country == pet.Address.Country);
        result.First().Pets.Should().ContainSingle(p => p.Address!.City == pet.Address.City);
        result.First().Pets.Should().ContainSingle(p => p.Address!.Street == pet.Address.Street);
        result.First().Pets.Should().ContainSingle(p => p.Address!.House == pet.Address.House);
        result.First().Pets.Should().ContainSingle(p => p.Address!.Flat == pet.Address.Flat);
   }
    
    [Fact]
    public async Task ShouldReturnVolunteerWithGivenPhoneNumber()
    {
        var volunteer = TestData.Volunteer;
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.SaveChangesAsync();
        
        var query = new VolunteerQueryModel
        {
            PhoneNumber = volunteer.PhoneNumber.Value
        };
        
        var result = await _sut.QueryVolunteers(query, CancellationToken.None);
        
        result.Should().HaveCount(1);
        result.First().PhoneNumber.Should().Be(volunteer.PhoneNumber.Value);
        result.First().FullName.Name.Should().Be(volunteer.FullName.Name);
        result.First().FullName.Surname.Should().Be(volunteer.FullName.Surname);
        result.First().FullName.Patronymic.Should().Be(volunteer.FullName.Patronymic);
        result.First().GeneralDescription.Should().Be(volunteer.GeneralDescription.Value);
        result.First().AgeExperience.Should().Be(volunteer.Experience.Years);
    }
    
    [Fact]
    public async Task ShouldReturnVolunteersWithGivenSpeciesIds()
    {
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.SaveChangesAsync();

        var query = new VolunteerQueryModel
        {
            SpeciesNames = [pet.AnimalType.SpeciesName.Value]
        };

        var result = await _sut.QueryVolunteers(query, CancellationToken.None);

        result.Should().NotBeEmpty();
        result.First(v => v.PhoneNumber == volunteer.PhoneNumber.Value).Pets
            .Should().Contain(p => p.PetName == pet.PetName.Value);
    }
    
    [Fact]
    public async Task ShouldReturnVolunteersWithGivenBreedIds()
    {
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        await using var dbContext = _fixture.GetVolunteerDbContext();
        await dbContext.AddAsync(volunteer);
        await dbContext.SaveChangesAsync();

        var query = new VolunteerQueryModel
        {
            BreedNames = [pet.AnimalType.BreedName.Value]
        };

        var result = await _sut.QueryVolunteers(query, CancellationToken.None);

        result.Should().NotBeEmpty();
        result.First(v => v.PhoneNumber == volunteer.PhoneNumber.Value).Pets
            .Should().ContainSingle(p => p.BreedName == pet.AnimalType.BreedName.Value);
    }
    
    [Fact]
    public async Task ShouldReturnVolunteersWithLimitAndOffset()
    {
        var volunteer1 = TestData.Volunteer;
        var volunteer2 = TestData.Volunteer;
        var volunteer3 = TestData.Volunteer;
        await using var dbContext = _fixture.GetVolunteerDbContext();
        List<Volunteer> volunteers = [volunteer1, volunteer2, volunteer3];
        await dbContext.AddRangeAsync(volunteers);
        await dbContext.SaveChangesAsync();
        
        var query = new VolunteerQueryModel
        {
            Limit = 2,
            Offset = 1
        };

        var volunteersInDb = dbContext.Volunteers.Select(v => v.FullName.Name).ToList();

        var result = await _sut.QueryVolunteers(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].FullName.Name.Should().Be(volunteersInDb[1]); 
        result[1].FullName.Name.Should().Be(volunteersInDb[2]); 
    }
}