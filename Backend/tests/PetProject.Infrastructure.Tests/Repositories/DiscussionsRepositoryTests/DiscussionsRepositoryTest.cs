using PetProject.Discussions.Infrastructure.Repositories;
using VolunteerRequests.Infrastructure.Repositories;

namespace PetProject.Infrastructure.Tests.Repositories.DiscussionsRepositoryTests;

[Collection(nameof(BaseTestFixture))]
public partial class DiscussionsRepositoryTest : IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;
    private readonly DiscussionsRepository _sut;


    public DiscussionsRepositoryTest(BaseTestFixture fixture)
    {
        _fixture = fixture;
        _sut = new DiscussionsRepository(fixture.GetDiscussionsDbContext());
        _fixture.ClearDatabaseAsync("discussions", "discussions", "members", "messages").Wait();
    }
}