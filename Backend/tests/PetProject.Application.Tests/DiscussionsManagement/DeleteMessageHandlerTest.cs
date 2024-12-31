using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.DeleteMessage;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.UpdateMessage;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;

namespace PetProject.Application.Tests.DiscussionsManagement;

public class DeleteMessageHandlerTest
{
    [Fact]
    public async Task DeleteMessage_ShouldDeleteMessage()
    {
        // Arrange
        var discussion = TestData.CreateDiscussion();
        var command = new DeleteMessageCommand()
        {
            UserId = discussion.Members.FirstMemberId,
            DiscussionId = discussion.Id.Id,
            MessageId = discussion.Messages.FirstOrDefault(x => x.UserId == discussion.Members.FirstMemberId)!.Id,
        };
        var handler = StubFactory.CreateDeleteMessageHandlerStub();
        
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