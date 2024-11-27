using FluentAssertions;
using PetProject.Application.Models;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;
using PetProject.SharedTestData;

namespace PetProject.Infrastructure.Tests.Repositories.SpeciesRepositoryTests;

public partial class SpeciesRepositoryTest
{
    [Fact]
    public async Task Query_ShouldReturnSpeciesWithGivenName()
    {
        var species = TestData.Species;
        await _sut.Add(species, CancellationToken.None);
        
        var speciesName = species.Name.Value;
        
        var query = new SpeciesQueryModel
        {
            SpeciesName = speciesName
        };
        
        var result = await _sut.Query(query, CancellationToken.None);
        
        result.Should().NotBeEmpty();
        result.FirstOrDefault(s => s.Name == speciesName)!.Name
            .Should().NotBeNull();
    }
    
    [Fact]
    public async Task Query_ShouldReturnSpeciesWithGivenBreedName()
    {
        var species = TestData.Species;
        await _sut.Add(species, CancellationToken.None);
        var breedName = species.Breeds.First().BreedName.Value;
        
        var query = new SpeciesQueryModel
        {
            BreedName = breedName
        };
        
        var result = await _sut.Query(query, CancellationToken.None);
        
        result.Should().NotBeEmpty();
        result.First(s => s.Breeds.Any(b => b.Name == breedName)).Breeds
            .Should().Contain(b => b.Name == breedName);
    }
    
    [Fact]
    public async Task Query_ShouldReturnSpeciesWithGivenSpeciesId()
    {
        var species = TestData.Species;
        await _sut.Add(species, CancellationToken.None);
        
        var speciesId = species.Id.Id;
        
        var query = new SpeciesQueryModel
        {
            SpeciesIds = [speciesId]
        };
        
        var result = await _sut.Query(query, CancellationToken.None);
        
        result.Should().NotBeEmpty();
        result.FirstOrDefault(s => s.Id == speciesId)!.Name
            .Should().NotBeNull();
    }
    
    [Fact]
    public async Task Query_ShouldReturnSpeciesWithGivenBreedId()
    {
        var species = TestData.Species;
        await _sut.Add(species, CancellationToken.None);
        var breedId = species.Breeds.First().Id.Id;
        
        var query = new SpeciesQueryModel
        {
            BreedIds = [breedId]
        };
        
        var result = await _sut.Query(query, CancellationToken.None);
        
        result.Should().NotBeEmpty();
        result.First(s => s.Breeds.Any(b => b.Id == breedId)).Breeds
            .Should().Contain(b => b.Id == breedId);
    }
    
    [Fact]
    public async Task ShouldReturnVolunteersWithLimitAndOffset()
    {
        var species1 = new Species(SpeciesId.Create(Guid.NewGuid()), SpeciesName.Create("Cat").Value, []);
        var species2 = new Species(SpeciesId.Create(Guid.NewGuid()), SpeciesName.Create("Dog").Value, []);
        var species3 = new Species(SpeciesId.Create(Guid.NewGuid()), SpeciesName.Create("testSpecies").Value, []);
        await _sut.Add(species1, CancellationToken.None);
        await _sut.Add(species2, CancellationToken.None);
        await _sut.Add(species3, CancellationToken.None);
        
        var query = new SpeciesQueryModel()
        {
            Limit = 2,
            Offset = 1
        };

        var result = await _sut.Query(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().NotContain(s => s.Id == species1.Id.Id); 
    }
}