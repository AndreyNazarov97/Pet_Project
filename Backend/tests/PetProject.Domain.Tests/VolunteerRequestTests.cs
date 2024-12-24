using FluentAssertions;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData;
using VolunteerRequests.Domain.Aggregate;
using VolunteerRequests.Domain.Enums;
using VolunteerRequests.Domain.ValueObjects;

namespace PetProject.Domain.Tests;

public class VolunteerRequestTests
{
    [Fact]
    public void Create_ShouldInitializeVolunteerRequest()
    {
        // Arrange
        var volunteerInfo = TestData.VolunteerInfo;
        var userId = 123L;

        // Act
        var result = VolunteerRequest.Create(volunteerInfo, userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var request = result.Value;
        request.Should().NotBeNull();
        request.VolunteerInfo.Should().Be(volunteerInfo);
        request.UserId.Should().Be(userId);
        request.RequestStatus.Should().Be(RequestStatus.New);
        request.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void TakeOnReview_ShouldUpdateStatusAndAssignAdmin()
    {
        // Arrange
        var volunteerRequest = CreateNew();
        var adminId = 456L;
        var discussionId = Guid.NewGuid();

        // Act
        var result = volunteerRequest.TakeOnReview(adminId, discussionId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteerRequest.RequestStatus.Should().Be(RequestStatus.OnReview);
        volunteerRequest.AdminId.Should().Be(adminId);
        volunteerRequest.DiscussionId.Should().Be(discussionId);
    }

    [Fact]
    public void TakeOnReview_ShouldFailIfNotNew()
    {
        // Arrange
        var volunteerRequest = CreateApproved();
        var adminId = 456L;
        var discussionId = Guid.NewGuid();

        // Act
        var result = volunteerRequest.TakeOnReview(adminId, discussionId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.RequestStatus.InvalidStatus());
    }

    [Fact]
    public void SendForRevision_ShouldUpdateStatusAndAddRejectionComment()
    {
        // Arrange
        var volunteerRequest = CreateOnReview();
        var rejectionComment = RejectionComment.Create("Needs more details").Value;

        // Act
        var result = volunteerRequest.SendForRevision(rejectionComment);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteerRequest.RequestStatus.Should().Be(RequestStatus.RevisionRequired);
        volunteerRequest.RejectionComment.Should().Be(rejectionComment);
    }

    [Fact]
    public void Approve_ShouldUpdateStatus()
    {
        // Arrange
        var volunteerRequest = CreateOnReview();

        // Act
        var result = volunteerRequest.Approve();

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteerRequest.RequestStatus.Should().Be(RequestStatus.Approved);
    }

    [Fact]
    public void Approve_ShouldFailIfInvalidStatus()
    {
        // Arrange
        var volunteerRequest = CreateNew();

        // Act
        var result = volunteerRequest.Approve();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.RequestStatus.InvalidStatus());
    }

    [Fact]
    public void Reject_ShouldUpdateStatusAndAddRejectionComment()
    {
        // Arrange
        var volunteerRequest = CreateOnReview();
        var rejectionComment =  RejectionComment.Create("Does not meet criteria").Value;

        // Act
        var result = volunteerRequest.Reject(rejectionComment);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteerRequest.RequestStatus.Should().Be(RequestStatus.Rejected);
        volunteerRequest.RejectionComment.Should().Be(rejectionComment);
        volunteerRequest.RejectionComment!.Value.Should().Be(rejectionComment.Value);
    }

    [Fact]
    public void Reject_ShouldFailIfInvalidStatus()
    {
        // Arrange
        var volunteerRequest = CreateNew();
        var rejectionComment = RejectionComment.Create("Invalid").Value;

        // Act
        var result = volunteerRequest.Reject(rejectionComment);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.RequestStatus.InvalidStatus());
    }

    private VolunteerRequest CreateNew()
    {
        var volunteerInfo = TestData.VolunteerInfo;
        var userId = 123L;
        
        var result = VolunteerRequest.Create(volunteerInfo, userId);
        return result.Value;
    }

    private VolunteerRequest CreateOnReview()
    {
        var volunteerRequest = CreateNew();
        var adminId = 456L;
        var discussionId = Guid.NewGuid();
        
       volunteerRequest.TakeOnReview(adminId, discussionId);
        return volunteerRequest;
    }

    private VolunteerRequest CreateApproved()
    {
        var volunteerRequest = CreateOnReview();
        volunteerRequest.Approve();
        return volunteerRequest;
    }
}