using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.CreateDiscussion;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Application.Tests.DiscussionsManagement;

public class CreateDiscussionHandlerTest
{
    private static CreateDiscussionCommand Command => new()
    {
        RelationId = Guid.NewGuid(),
        FirstMemberId = Random.Long,
        SecondMemberId = Random.Long
    };
    
    [Fact]
    public async Task ShouldCreateDiscussion()
    {
        // Arrange
        var command = Command;
        var handler = StubFactory.CreateCreateDiscussionHandlerStub();

        // Act
        handler.AccountsContractMock.SetupGetById(TestData.UserDto with { Id = command.FirstMemberId});
        handler.DiscussionsRepositoryMock.SetupGetByRelationId(Errors.General.NotFound(command.RelationId));
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.AccountsContractMock.Verify(x => x.GetUserById(
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));
        handler.DiscussionsRepositoryMock.Verify(repo => repo.Add(
                It.IsAny<Discussion>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}