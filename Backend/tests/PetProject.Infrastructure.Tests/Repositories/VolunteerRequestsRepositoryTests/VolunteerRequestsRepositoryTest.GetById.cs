using FluentAssertions;
using PetProject.SharedTestData;
using VolunteerRequests.Domain.Aggregate;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Infrastructure.Tests.Repositories.VolunteerRequestsRepositoryTests;

public partial class VolunteerRequestsRepositoryTest
{
    [Fact]
    public async Task ShouldReturnError_WhenVolunteerNotExists()
    {
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        var result = await _sut.GetById(request.Id, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("record.not.found");
    }

    [Fact]
    public async Task ShouldReturnVolunteerWithGivenId()
    {
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        await _sut.Add(request, CancellationToken.None);

        var result = await _sut.GetById(request.Id, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(request.Id);
    }
}