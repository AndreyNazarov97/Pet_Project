using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;
using VolunteerRequests.Domain.Aggregate;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Application.Tests.RequestsManagement;

public class TakeVolunteerRequestOnReviewHandlerTest
{
    private static TakeVolunteerRequestOnReviewCommand Command => new()
    {
        VolunteerRequestId = Guid.NewGuid(),
        AdminId = Random.Long,
    };
    
    [Fact]
    public async Task ShouldTakeVolunteerRequestOnReview()
    {
        // Arrange
        var command = Command;
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        var handler = StubFactory.CreateTakeVolunteerRequestOnReviewHandlerStub();
        
        // Act
        handler.VolunteerRequestsRepositoryMock.SetupGetById( request);
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.VolunteerRequestsRepositoryMock.Verify(repo => repo.GetById(
            It.IsAny<VolunteerRequestId>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}