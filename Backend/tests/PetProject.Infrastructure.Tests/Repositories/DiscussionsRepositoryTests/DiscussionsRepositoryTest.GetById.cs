using FluentAssertions;
using PetProject.SharedTestData;

namespace PetProject.Infrastructure.Tests.Repositories.DiscussionsRepositoryTests;

public partial class DiscussionsRepositoryTest
{ 
    [Fact]
    public async Task ShouldReturnError_WhenDiscussionNotExists()
    {
        var discussion = TestData.CreateDiscussion();
        var result = await _sut.GetById(discussion.Id, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("record.not.found");
    }

    [Fact]
    public async Task ShouldReturnDiscussionWithGivenId()
    {
        var discussion = TestData.CreateDiscussion();
        await _sut.Add(discussion, CancellationToken.None);

       
        var result = await _sut.GetById(discussion.Id, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(discussion.Id);
        result.Value.Members.FirstMemberId.Should().Be(discussion.Members.FirstMemberId);
        result.Value.Members.SecondMemberId.Should().Be(discussion.Members.SecondMemberId);
        result.Value.RelationId.Should().Be(discussion.RelationId);
        result.Value.Messages[0].Should().Be(discussion.Messages[0]);
    }
}