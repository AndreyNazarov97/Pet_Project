using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using VolunteerRequests.Application.RequestsManagement.Commands.Reject;
using VolunteerRequests.Domain.Aggregate;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Application.Tests.RequestsManagement;

public class RejectVolunteerRequestHandlerTest
{
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
        var handler = StubFactory.CreateRejectVolunteerRequestHandlerStub();
        
        // Act
        handler.VolunteerRequestsRepositoryMock.SetupGetById(Errors.General.NotFound(command.VolunteerRequestId));
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().Contain(Errors.General.NotFound(command.VolunteerRequestId));
         
        handler.VolunteerRequestsRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerRequestId>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenApprovedByWrongAdmin()
    {
        // Arrange
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        var command = Command with{VolunteerRequestId = request.Id};
        request.TakeOnReview(command.AdminId + 1, Guid.NewGuid());
        
        var handler = StubFactory.CreateRejectVolunteerRequestHandlerStub();
        
        // Act
        handler.VolunteerRequestsRepositoryMock.SetupGetById(request);
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().Contain(Errors.VolunteerRequests.AccessDenied());
        
        handler.VolunteerRequestsRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerRequestId>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldApproveVolunteerRequest()
    {
        // Arrange
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        var command = Command with{VolunteerRequestId = request.Id};
        request.TakeOnReview(command.AdminId, Guid.NewGuid());
        
        var handler = StubFactory.CreateRejectVolunteerRequestHandlerStub();
        
        // Act
        handler.VolunteerRequestsRepositoryMock.SetupGetById(request);
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.VolunteerRequestsRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerRequestId>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}