using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.SendMessage;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.DiscussionsManagement;

public class SendMessageHandlerTest : DiscussionsManagementTestBase
{
    private readonly IRequestHandler<SendMessageCommand, Result<Guid, ErrorList>> _sut;

    public SendMessageHandlerTest(DiscussionsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<SendMessageCommand, Result<Guid, ErrorList>>>();
    }

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
        
        await SeedDiscussion(discussion);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}