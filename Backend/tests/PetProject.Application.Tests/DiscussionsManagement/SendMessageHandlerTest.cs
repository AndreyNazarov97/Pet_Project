using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.SendMessage;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Application.Tests.DiscussionsManagement;

public class SendMessageHandlerTest
{
    [Fact]
    public async Task SendMessage_ShouldSendMessageToDiscussion()
    {
        // Arrange
        var discussion = TestData.CreateDiscussion();
        var command = new SendMessageCommand
        {
            UserId = discussion.Members.FirstMemberId,
            DiscussionId = discussion.Id.Id,
            Text = Random.Words
        };
        var handler = StubFactory.CreateSendMessageHandlerStub();
        
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