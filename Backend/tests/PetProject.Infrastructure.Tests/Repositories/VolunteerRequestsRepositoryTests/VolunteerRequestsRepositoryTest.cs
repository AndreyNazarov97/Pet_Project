using VolunteerRequests.Infrastructure.Repositories;

namespace PetProject.Infrastructure.Tests.Repositories.VolunteerRequestsRepositoryTests;

[Collection(nameof(BaseTestFixture))]
public partial class VolunteerRequestsRepositoryTest : IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;
    private readonly VolunteerRequestsRepository _sut;


    public VolunteerRequestsRepositoryTest(BaseTestFixture fixture)
    {
        _fixture = fixture;
        _sut = new VolunteerRequestsRepository(fixture.GetVolunteerRequestsDbContext());
        _fixture.ClearDatabaseAsync("volunteers_requests", "volunteer_requests").Wait();
    }
}