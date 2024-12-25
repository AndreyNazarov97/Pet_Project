using FluentAssertions;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.Discussions.Domain.Entity;
using PetProject.Discussions.Domain.Enums;
using PetProject.Discussions.Domain.ValueObjects;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Domain.Tests;

public class DiscussionTests
{
    [Fact]
    public void Create_ShouldReturnDiscussion_WhenValidInputsProvided()
    {
        // Arrange
        var relationId = Guid.NewGuid();
        var users = Users.Create(1, 2).Value;

        // Act
        var result = Discussion.Create(relationId, users);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.RelationId.Should().Be(relationId);
        result.Value.Users.Should().Be(users);
        result.Value.Status.Should().Be(DiscussionStatus.Opened);
    }

    [Fact]
    public void Create_ShouldFail_WhenRelationIdIsEmpty()
    {
        // Arrange
        var relationId = Guid.Empty;
        var users = Users.Create(1, 2).Value;

        // Act
        var result = Discussion.Create(relationId, users);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void AddMessage_ShouldAddMessage_WhenUserIsInDiscussion()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var message = CreateMessage(discussion.Users.FirstMemberId);

        // Act
        var result = discussion.AddMessage(message);

        // Assert
        result.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().Contain(message);
    }

    [Fact]
    public void AddMessage_ShouldFail_WhenUserIsNotInDiscussion()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var invalidUserId = discussion.Users.FirstMemberId + discussion.Users.SecondMemberId;
        var message = CreateMessage(invalidUserId);

        // Act
        var result = discussion.AddMessage(message);

        // Assert
        result.IsFailure.Should().BeTrue();
        discussion.Messages.Should().BeEmpty();
    }

    [Fact]
    public void DeleteMessage_ShouldRemoveMessage_WhenUserOwnsMessage()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var message = CreateMessage(discussion.Users.FirstMemberId);
        discussion.AddMessage(message);

        // Act
        var result = discussion.DeleteMessage(discussion.Users.FirstMemberId, message.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().BeEmpty();
    }

    [Fact]
    public void DeleteMessage_ShouldFail_WhenUserDoesNotOwnMessage()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var message = CreateMessage(discussion.Users.FirstMemberId);
        discussion.AddMessage(message);

        // Act
        var result = discussion.DeleteMessage(discussion.Users.SecondMemberId, message.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        discussion.Messages.Should().Contain(message);
    }

    [Fact]
    public void EditMessage_ShouldUpdateMessage_WhenUserOwnsMessage()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var message = CreateMessage(discussion.Users.FirstMemberId);
        discussion.AddMessage(message);
        var newText = Text.Create("Updated").Value;

        // Act
        var result = discussion.EditMessage(discussion.Users.FirstMemberId, message.Id, newText);

        // Assert
        result.IsSuccess.Should().BeTrue();
        discussion.Messages.Single().Text.Should().Be(newText);
        discussion.Messages.Single().IsEdited.Should().BeTrue();
    }

    [Fact]
    public void EditMessage_ShouldFail_WhenUserDoesNotOwnMessage()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var message = CreateMessage(discussion.Users.FirstMemberId);
        discussion.AddMessage(message);
        var newText = Text.Create("Updated").Value;

        // Act
        var result = discussion.EditMessage(discussion.Users.SecondMemberId, message.Id, newText);

        // Assert
        result.IsFailure.Should().BeTrue();
        discussion.Messages.Single().Text.Value.Should().Be("Hello");
    }

    [Fact]
    public void CloseDiscussion_ShouldSetStatusToClosed_WhenUserIsInDiscussion()
    {
        // Arrange
        var discussion = CreateDiscussion();

        // Act
        var result = discussion.CloseDiscussion(discussion.Users.FirstMemberId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        discussion.Status.Should().Be(DiscussionStatus.Closed);
    }

    [Fact]
    public void CloseDiscussion_ShouldFail_WhenUserIsNotInDiscussion()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var invalidUserId = discussion.Users.FirstMemberId + discussion.Users.SecondMemberId;

        // Act
        var result = discussion.CloseDiscussion(invalidUserId);

        // Assert
        result.IsFailure.Should().BeTrue();
        discussion.Status.Should().Be(DiscussionStatus.Opened);
    }

    [Fact]
    public void CloseDiscussion_ShouldFail_WhenDiscussionIsAlreadyClosed()
    {
        // Arrange
        var discussion = CreateDiscussion();
        discussion.CloseDiscussion(discussion.Users.FirstMemberId);

        // Act
        var result = discussion.CloseDiscussion(discussion.Users.FirstMemberId);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
    
    private Discussion CreateDiscussion()
    {
        return Discussion
            .Create(Guid.NewGuid(), Users.Create(Random.Long, Random.Long).Value).Value;
    }
    
    private Message CreateMessage(long userId)
    {
        return Message
            .Create(Text.Create("Hello").Value, userId).Value;
    }
}
