using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.SendMessage;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.UpdateMessage;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Application.Tests.DiscussionsManagement;

public class UpdateMessageHandlerTest
{
    [Fact]
    public async Task UpdateMessage_ShouldUpdateMessage()
    {
        // Arrange
        var discussion = TestData.CreateDiscussion();
        var command = new UpdateMessageCommand
        {
            UserId = discussion.Members.FirstMemberId,
            DiscussionId = discussion.Id.Id,
            MessageId = discussion.Messages.FirstOrDefault(x => x.UserId == discussion.Members.FirstMemberId)!.Id,
            Text = Random.Words,
        };
        var handler = StubFactory.CreateUpdateMessageHandlerStub();
        
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