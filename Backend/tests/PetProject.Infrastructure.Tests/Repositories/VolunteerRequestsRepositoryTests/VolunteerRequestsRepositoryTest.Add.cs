using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PetProject.SharedTestData;
using VolunteerRequests.Domain.Aggregate;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Infrastructure.Tests.Repositories.VolunteerRequestsRepositoryTests;

public partial class VolunteerRequestsRepositoryTest
{
    [Fact]
    public async Task Add_ShouldAddVolunteerToDatabase()
    {
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;

        var result = await _sut.Add(request, CancellationToken.None);

        var addedRequest = await _sut.GetById(request.Id, CancellationToken.None);

        await using var dbContext = _fixture.GetVolunteerRequestsDbContext();
        var requestsId = await dbContext.VolunteerRequests
            .Where(v => v.Id == request.Id)
            .Select(v => v.Id.Id)
            .ToListAsync();

        result.Should().Be(request.Id.Id);
        addedRequest.IsSuccess.Should().BeTrue();
        requestsId.Should()
            .HaveCount(1)
            .And.Contain(request.Id.Id);
    }
}