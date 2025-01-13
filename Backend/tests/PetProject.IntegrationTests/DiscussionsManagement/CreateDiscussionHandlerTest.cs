using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.CreateDiscussion;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.DiscussionsManagement;

public class CreateDiscussionHandlerTest: DiscussionsManagementTestBase
{
    private readonly IRequestHandler<CreateDiscussionCommand, Result<Discussion, ErrorList>> _sut;

    public CreateDiscussionHandlerTest(DiscussionsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<CreateDiscussionCommand, Result<Discussion, ErrorList>>>();
    }
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

        // Act
        _factory.SetupGetUserById(command.FirstMemberId, TestData.UserDto);
        _factory.SetupGetUserById(command.SecondMemberId, TestData.UserDto);
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}