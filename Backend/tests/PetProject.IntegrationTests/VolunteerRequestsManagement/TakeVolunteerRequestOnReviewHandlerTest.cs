using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.VolunteerRequestsManagement;

public class TakeVolunteerRequestOnReviewHandlerTest : VolunteerRequestsManagementTestsBase
{
    private readonly IRequestHandler<TakeVolunteerRequestOnReviewCommand, Result<DiscussionId, ErrorList>> _sut;

    public TakeVolunteerRequestOnReviewHandlerTest(VolunteerRequestsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<TakeVolunteerRequestOnReviewCommand, Result<DiscussionId, ErrorList>>>();
    }

    private static TakeVolunteerRequestOnReviewCommand Command => new()
    {
        VolunteerRequestId = Guid.NewGuid(),
        AdminId = Random.Long,
    };
    
    [Fact]
    public async Task ShouldTakeVolunteerRequestOnReview()
    {
        // Arrange
        var discussion = TestData.CreateDiscussion();
        var request = TestData.VolunteerRequest;
        var command = Command with{VolunteerRequestId = request.Id};

        await SeedVolunteerRequest(request);
        
        // Act
        _factory.SetupCreate(discussion);
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}