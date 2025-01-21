using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData;
using VolunteerRequests.Application.RequestsManagement.Commands.Approve;
using VolunteerRequests.Domain.Events;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.VolunteerRequestsManagement;

public class ApproveVolunteerRequestHandlerTest : VolunteerRequestsManagementTestsBase
{
    private readonly IRequestHandler<ApproveVolunteerRequestCommand, UnitResult<ErrorList>> _sut;

    public ApproveVolunteerRequestHandlerTest(VolunteerRequestsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<ApproveVolunteerRequestCommand, UnitResult<ErrorList>>>();
    }

    private static ApproveVolunteerRequestCommand Command => new()
    {
        VolunteerRequestId = Guid.NewGuid(),
        AdminId = Random.Long,
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
        var command = Command with {VolunteerRequestId = request.Id};
        request.TakeOnReview(command.AdminId + 1, Guid.NewGuid());
        await SeedVolunteerRequest(request);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().Contain(Errors.VolunteerRequests.AccessDenied());
    }
    
    [Fact]
    public async Task Handle_ShouldApproveVolunteerRequest()
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
    }
}