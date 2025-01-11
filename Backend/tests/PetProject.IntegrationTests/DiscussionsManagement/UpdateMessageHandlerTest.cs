using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.UpdateMessage;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.DiscussionsManagement;

public class UpdateMessageHandlerTest : DiscussionsManagementTestBase
{
    private readonly IRequestHandler<UpdateMessageCommand, Result<Guid, ErrorList>> _sut;

    public UpdateMessageHandlerTest(DiscussionsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<UpdateMessageCommand, Result<Guid, ErrorList>>>();
    }

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
        
        await SeedDiscussion(discussion);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}