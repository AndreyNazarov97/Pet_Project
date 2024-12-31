using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PetProject.SharedTestData;

namespace PetProject.Infrastructure.Tests.Repositories.DiscussionsRepositoryTests;

public partial class DiscussionsRepositoryTest
{
    [Fact]
    public async Task Add_ShouldAddVolunteerToDatabase()
    {
        var discussion = TestData.CreateDiscussion();

        var result = await _sut.Add(discussion, CancellationToken.None);

        var addedDiscussion = await _sut.GetById(discussion.Id, CancellationToken.None);

        await using var dbContext = _fixture.GetDiscussionsDbContext();
        var discussionsId = await dbContext.Discussions
            .Where(v => v.Id == discussion.Id)
            .Select(v => v.Id.Id)
            .ToListAsync();

        result.Id.Id.Should().Be(discussion.Id.Id);
        addedDiscussion.IsSuccess.Should().BeTrue();
        discussionsId.Should()
            .HaveCount(1)
            .And.Contain(discussion.Id.Id);
    }
}