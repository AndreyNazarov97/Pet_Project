using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData;
using VolunteerRequests.Application.RequestsManagement.Commands.Reject;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.VolunteerRequestsManagement;

public class RejectVolunteerRequestHandlerTest : VolunteerRequestsManagementTestsBase
{
    private readonly IRequestHandler<RejectVolunteerRequestCommand, UnitResult<ErrorList>> _sut;

    public RejectVolunteerRequestHandlerTest(VolunteerRequestsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<RejectVolunteerRequestCommand, UnitResult<ErrorList>>>();
    }

    private static RejectVolunteerRequestCommand Command => new()
    {
        VolunteerRequestId = Guid.NewGuid(),
        AdminId = Random.Long,
        RejectionComment = Random.Words
    };
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerRequestNotFound()
    {
        // Arrange
        var command = Command;
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().Contain(Errors.General.NotFound(command.VolunteerRequestId));
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenApprovedByWrongAdmin()
    {
        // Arrange
        var request = TestData.VolunteerRequest;
        var command = Command with{VolunteerRequestId = request.Id};
        request.TakeOnReview(command.AdminId + 1, Guid.NewGuid());
        
        await SeedVolunteerRequest(request);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().Contain(Errors.VolunteerRequests.AccessDenied());
    }
    
    [Fact]
    public async Task Handle_ShouldRejectVolunteerRequest()
    {
        // Arrange
        var request = TestData.VolunteerRequest;
        var command = Command with{VolunteerRequestId = request.Id};
        request.TakeOnReview(command.AdminId, Guid.NewGuid());
        
        await SeedVolunteerRequest(request);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var userRestriction = await _volunteerRequestsDbContext.UserRestrictions
            .FirstOrDefaultAsync();
        
        userRestriction.Should().NotBeNull();
        userRestriction!.UserId.Should().Be(request.UserId);
    }
}