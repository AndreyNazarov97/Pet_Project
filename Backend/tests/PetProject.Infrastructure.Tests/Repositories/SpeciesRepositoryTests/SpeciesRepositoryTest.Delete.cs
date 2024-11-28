using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PetProject.SharedTestData;

namespace PetProject.Infrastructure.Tests.Repositories.SpeciesRepositoryTests;

public partial class SpeciesRepositoryTest
{
    [Fact]
    public async Task Delete_ShouldDeleteSpeciesFromDatabase()
    {
        var species = TestData.Species;

        await _sut.Add(species, CancellationToken.None);

        var result = await _sut.Delete(species.Id, CancellationToken.None);
        
        await using var dbContext = _fixture.GetSpeciesDbContext();
        var speciesInDatabase = await dbContext.Species
            .Where(s => s.Id == species.Id)
            .Select(s => s.Name.Value)
            .ToListAsync();
        
        result.IsSuccess.Should().BeTrue();
        speciesInDatabase.Should().BeEmpty();
    }
}