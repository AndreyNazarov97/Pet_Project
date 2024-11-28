using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PetProject.SharedTestData;

namespace PetProject.Infrastructure.Tests.Repositories.SpeciesRepositoryTests;

public partial class SpeciesRepositoryTest
{
    [Fact]
    public async Task Add_ShouldAddSpeciesToDatabase()
    {
        var species = TestData.Species;

        var result = await _sut.Add(species, CancellationToken.None);
        
        await using var dbContext = _fixture.GetSpeciesDbContext();
        var volunteersPhones = await dbContext.Species
            .Where(s => s.Id == species.Id)
            .Select(s => s.Name.Value)
            .ToListAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(species.Id);
        volunteersPhones.Should()
            .HaveCount(1)
            .And.Contain(species.Name.Value);
    }
}