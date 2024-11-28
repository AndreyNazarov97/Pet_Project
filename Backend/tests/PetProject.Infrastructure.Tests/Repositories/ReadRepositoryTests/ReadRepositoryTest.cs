using PetProject.Core.Database.Repository;
using PetProject.SpeciesManagement.Infrastructure.DbContexts;

namespace PetProject.Infrastructure.Tests.Repositories.ReadRepositoryTests;
[Collection(nameof(BaseTestFixture))]
public partial class ReadRepositoryTest :  IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;
    private readonly ReadRepository _sut;
    

    public ReadRepositoryTest(BaseTestFixture fixture)
    {
        _fixture = fixture;
        _sut = new ReadRepository(fixture.GetConnectionFactory());
        _fixture.ClearDatabaseAsync("volunteers", "pets" ,"species", "breeds").Wait(); 
    }
}