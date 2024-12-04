using PetProject.VolunteerManagement.Infrastructure.Repositories;

namespace PetProject.Infrastructure.Tests.Repositories.VolunteerRepositoryTests;
[Collection(nameof(BaseTestFixture))]
public partial class VolunteerRepositoryTest : IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;
    private readonly VolunteersRepository _sut;
    

    public VolunteerRepositoryTest(BaseTestFixture fixture)
    {
        _fixture = fixture;
        _sut = new VolunteersRepository(fixture.GetVolunteerDbContext());
        _fixture.ClearDatabaseAsync( "volunteers","volunteers", "pets").Wait(); 
    }
    
    
    
}

