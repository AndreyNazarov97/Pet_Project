using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.CloseDiscussion;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;

namespace PetProject.Application.Tests.DiscussionsManagement;

public class CloseDiscussionHandlerTest
{
    [Fact]
    public async Task ShouldCloseDiscussion()
    {
        // Arrange
        var discussion = TestData.CreateDiscussion();
        var command = new CloseDiscussionCommand()
        {
            DiscussionId = discussion.Id,
            UserId = discussion.Members.FirstMemberId
        };
        var handler = StubFactory.CreateCloseDiscussionHandlerStub();

        // Act
        handler.DiscussionsRepositoryMock.SetupGetById(discussion);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.DiscussionsRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<DiscussionId>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}