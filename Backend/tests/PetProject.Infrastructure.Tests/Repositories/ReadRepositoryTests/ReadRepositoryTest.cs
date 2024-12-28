using PetProject.Core.Database.Repository;

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
        _fixture.ClearDatabaseAsync( "volunteers","volunteers", "pets").Wait(); 
        _fixture.ClearDatabaseAsync( "species" ,"species", "breeds").Wait(); 
        _fixture.ClearDatabaseAsync( "volunteers_requests" ,"volunteer_requests").Wait();
    }
}