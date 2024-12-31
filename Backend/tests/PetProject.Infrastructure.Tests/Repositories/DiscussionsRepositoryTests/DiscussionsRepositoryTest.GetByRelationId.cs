using FluentAssertions;
using PetProject.SharedTestData;

namespace PetProject.Infrastructure.Tests.Repositories.DiscussionsRepositoryTests;

public partial class DiscussionsRepositoryTest
{
    [Fact]
    public async Task ShouldReturnError_WhenDiscussionWithRelationIdNotExists()
    {
        var discussion = TestData.CreateDiscussion();
        var result = await _sut.GetByRelationId(discussion.RelationId, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("record.not.found");
    }

    [Fact]
    public async Task ShouldReturnDiscussionWithGivenRelationId()
    {
        var discussion = TestData.CreateDiscussion();
        await _sut.Add(discussion, CancellationToken.None);

       
        var result = await _sut.GetByRelationId(discussion.RelationId, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(discussion.Id);
    }
}