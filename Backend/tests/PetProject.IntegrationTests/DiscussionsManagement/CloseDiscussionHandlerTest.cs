using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.CloseDiscussion;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData;

namespace PetProject.IntegrationTests.DiscussionsManagement;

public class CloseDiscussionHandlerTest : DiscussionsManagementTestBase
{
    private readonly IRequestHandler<CloseDiscussionCommand, UnitResult<ErrorList>> _sut;

    public CloseDiscussionHandlerTest(DiscussionsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<CloseDiscussionCommand, UnitResult<ErrorList>>>();
    }

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
        
        await SeedDiscussion(discussion);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}