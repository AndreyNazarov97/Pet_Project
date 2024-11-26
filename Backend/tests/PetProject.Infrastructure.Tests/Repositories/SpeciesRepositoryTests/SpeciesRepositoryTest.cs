using PetProject.Infrastructure.Postgres.Repositories;

namespace PetProject.Infrastructure.Tests.Repositories.SpeciesRepositoryTests;
[Collection(nameof(BaseTestFixture))]
public partial class SpeciesRepositoryTest : IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;
    private readonly SpeciesRepository _sut;
    

    public SpeciesRepositoryTest(BaseTestFixture fixture)
    {
        _fixture = fixture;
        _sut = new SpeciesRepository(fixture.GetDbContext(), fixture.GetConnectionFactory());
    }
}