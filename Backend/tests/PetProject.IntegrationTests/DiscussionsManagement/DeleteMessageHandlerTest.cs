using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.DeleteMessage;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData;

namespace PetProject.IntegrationTests.DiscussionsManagement;

public class DeleteMessageHandlerTest : DiscussionsManagementTestBase
{
    private readonly IRequestHandler<DeleteMessageCommand, UnitResult<ErrorList>> _sut;

    public DeleteMessageHandlerTest(DiscussionsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<DeleteMessageCommand, UnitResult<ErrorList>>>();
    }

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
        
        await SeedDiscussion(discussion);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}