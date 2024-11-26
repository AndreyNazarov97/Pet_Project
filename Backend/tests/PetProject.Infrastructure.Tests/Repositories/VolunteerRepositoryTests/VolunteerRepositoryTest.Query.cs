using FluentAssertions;
using PetProject.Application.Models;
using PetProject.SharedTestData;

namespace PetProject.Infrastructure.Tests.Repositories.VolunteerRepositoryTests;

public partial class VolunteerRepositoryTest
{
    [Fact]
    public async Task ShouldReturnVolunteersWithGivenPetIds()
    {
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        await _sut.Add(volunteer, CancellationToken.None);

        var query = new VolunteerQueryModel
        {
            PetIds = [pet.Id.Id]
        };

        var result = await _sut.Query(query, CancellationToken.None);

        result.Should().HaveCount(1);
        result.First().Pets.Should().ContainSingle(p => p.PetName == pet.PetName.Value);
    }
    
    [Fact]
    public async Task ShouldReturnVolunteerWithGivenPhoneNumber()
    {
        var volunteer = TestData.Volunteer;
        await _sut.Add(volunteer, CancellationToken.None);
        var query = new VolunteerQueryModel
        {
            PhoneNumber = volunteer.PhoneNumber.Value
        };
        
        var result = await _sut.Query(query, CancellationToken.None);
        
        result.Should().HaveCount(1);
        result.First().PhoneNumber.Should().Be(volunteer.PhoneNumber.Value);
    }
    
    [Fact]
    public async Task ShouldReturnVolunteersWithGivenSpeciesIds()
    {
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        await _sut.Add(volunteer, CancellationToken.None);

        var query = new VolunteerQueryModel
        {
            SpeciesNames = [pet.AnimalType.SpeciesName.Value]
        };

        var result = await _sut.Query(query, CancellationToken.None);

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
        await _sut.Add(volunteer, CancellationToken.None);

        var query = new VolunteerQueryModel
        {
            BreedNames = [pet.AnimalType.BreedName.Value]
        };

        var result = await _sut.Query(query, CancellationToken.None);

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
        await _sut.Add(volunteer1, CancellationToken.None);
        await _sut.Add(volunteer2, CancellationToken.None);
        await _sut.Add(volunteer3, CancellationToken.None);

        var query = new VolunteerQueryModel
        {
            Limit = 2,
            Offset = 1
        };

        var result = await _sut.Query(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().NotContain(v => v.PhoneNumber == volunteer1.PhoneNumber.Value); 
    }
    
}