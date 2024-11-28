using FluentAssertions;
using PetProject.Core.Database.Models;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.SharedTestData;
using PetProject.SpeciesManagement.Domain.Aggregate;

namespace PetProject.Infrastructure.Tests.Repositories.ReadRepositoryTests;

public partial class ReadRepositoryTest
{
    [Fact]
    public async Task Query_ShouldReturnSpeciesWithGivenName()
    {
        var species = TestData.Species;
        await using var dbContext = _fixture.GetSpeciesDbContext();
        await dbContext.AddAsync(species);
        await dbContext.SaveChangesAsync();
        
        var speciesName = species.Name.Value;
        
        var query = new SpeciesQueryModel
        {
            SpeciesName = speciesName
        };
        
        var result = await _sut.QuerySpecies(query, CancellationToken.None);
        
        result.Should().NotBeEmpty();
        result.FirstOrDefault(s => s.Name == speciesName)!.Name
            .Should().NotBeNull();
    }
    
    [Fact]
    public async Task Query_ShouldReturnSpeciesWithGivenBreedName()
    {
        var species = TestData.Species;
        await using var dbContext = _fixture.GetSpeciesDbContext();
        await dbContext.AddAsync(species);
        await dbContext.SaveChangesAsync();
        
        var breedName = species.Breeds.First().BreedName.Value;
        
        var query = new SpeciesQueryModel
        {
            BreedName = breedName
        };
        
        var result = await _sut.QuerySpecies(query, CancellationToken.None);
        
        result.Should().NotBeEmpty();
        result.First(s => s.Breeds.Any(b => b.Name == breedName)).Breeds
            .Should().Contain(b => b.Name == breedName);
    }
    
    [Fact]
    public async Task Query_ShouldReturnSpeciesWithGivenSpeciesId()
    {
        var species = TestData.Species;
        await using var dbContext = _fixture.GetSpeciesDbContext();
        await dbContext.AddAsync(species);
        await dbContext.SaveChangesAsync();
        
        var speciesId = species.Id.Id;
        
        var query = new SpeciesQueryModel
        {
            SpeciesIds = [speciesId]
        };
        
        var result = await _sut.QuerySpecies(query, CancellationToken.None);
        
        result.Should().NotBeEmpty();
        result.FirstOrDefault(s => s.Id == speciesId)!.Name
            .Should().NotBeNull();
    }
    
    [Fact]
    public async Task Query_ShouldReturnSpeciesWithGivenBreedId()
    {
        var species = TestData.Species;
        await using var dbContext = _fixture.GetSpeciesDbContext();
        await dbContext.AddAsync(species);
        await dbContext.SaveChangesAsync();
        
        var breedId = species.Breeds.First().Id.Id;
        
        var query = new SpeciesQueryModel
        {
            BreedIds = [breedId]
        };
        
        var result = await _sut.QuerySpecies(query, CancellationToken.None);
        
        result.Should().NotBeEmpty();
        result.First(s => s.Breeds.Any(b => b.Id == breedId)).Breeds
            .Should().Contain(b => b.Id == breedId);
    }
    
    [Fact]
    public async Task ShouldReturnSpeciesWithLimitAndOffset()
    {
        var species1 = new Species(SpeciesId.Create(Guid.NewGuid()), SpeciesName.Create("Cat").Value, []);
        var species2 = new Species(SpeciesId.Create(Guid.NewGuid()), SpeciesName.Create("Dog").Value, []);
        var species3 = new Species(SpeciesId.Create(Guid.NewGuid()), SpeciesName.Create("testSpecies").Value, []);
        await using var dbContext = _fixture.GetSpeciesDbContext();
        List<Species> species = [species1, species2, species3]; 
        await dbContext.AddRangeAsync(species);
        await dbContext.SaveChangesAsync();
        
        var query = new SpeciesQueryModel()
        {
            SortBy = "SpeciesName",
            Limit = 2,
            Offset = 1
        };

        var sortedSpeciesNames = species
            .Select(s => s.Name.Value)
            .OrderBy(s => s)
            .ToList();

        var result = await _sut.QuerySpecies(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Name.Should().Be(sortedSpeciesNames[1]);
        result[1].Name.Should().Be(sortedSpeciesNames[2]);
    }
}