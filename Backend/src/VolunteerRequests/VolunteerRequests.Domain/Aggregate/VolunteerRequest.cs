using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.Common;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using VolunteerRequests.Domain.Enums;
using VolunteerRequests.Domain.ValueObjects;

namespace VolunteerRequests.Domain.Aggregate;

public class VolunteerRequest : AggregateRoot<VolunteerRequestId>
{
    private VolunteerRequest(VolunteerRequestId id) : base(id)
    {
    }

    public VolunteerInfo VolunteerInfo { get; private set; }
    public RequestStatus RequestStatus { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public long? AdminId { get; private set; }
    public long UserId { get; private set; }
    public Guid? DiscussionId { get; private set; }
    public RejectionComment RejectionComment { get; private set; }

    private VolunteerRequest(
        VolunteerRequestId id,
        VolunteerInfo volunteerInfo,
        RequestStatus requestStatus,
        DateTimeOffset createdAt,
        long userId,
        long? adminId = null,
        Guid? discussionId = null) : base(id)
    {
        VolunteerInfo = volunteerInfo;
        RequestStatus = requestStatus;
        CreatedAt = createdAt;
        AdminId = adminId;
        UserId = userId;
        DiscussionId = discussionId;
        RejectionComment = RejectionComment.Empty;
    }

    public static Result<VolunteerRequest, Error> Create(
        VolunteerInfo volunteerInfo,
        long userId)
    {
        if (userId <= 0)
            return Errors.General.ValueIsInvalid(nameof(userId));
        
        var requestId = VolunteerRequestId.NewId();

        var request = new VolunteerRequest(
            requestId,
            volunteerInfo,
            RequestStatus.New,
            DateTimeOffset.UtcNow,
            userId);

        return request;
    }
    
    public UnitResult<Error> UpdateInfo(VolunteerInfo volunteerInfo)
    {
        if (RequestStatus != RequestStatus.RevisionRequired)
            return Errors.VolunteerRequests.InvalidStatus();
        
        VolunteerInfo = volunteerInfo;
        return Result.Success<Error>();
    }

    public UnitResult<Error> TakeOnReview(long adminId, Guid discussionId)
    {
        if (RequestStatus != RequestStatus.New)
            return Errors.VolunteerRequests.InvalidStatus();

        RequestStatus = RequestStatus.OnReview;
        AdminId = adminId;
        DiscussionId = discussionId;

        return Result.Success<Error>();
    }

    public UnitResult<Error> SendForRevision(RejectionComment rejectionComment)
    {
        if (RequestStatus == RequestStatus.Approved || RequestStatus == RequestStatus.Rejected)
            return Errors.VolunteerRequests.InvalidStatus();
        
        RequestStatus = RequestStatus.RevisionRequired;
        RejectionComment = rejectionComment;
        
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> Approve()
    {
        if (RequestStatus is RequestStatus.Approved or RequestStatus.Rejected or RequestStatus.New)
            return Errors.VolunteerRequests.InvalidStatus();
        
        RequestStatus = RequestStatus.Approved;
        RejectionComment = RejectionComment.Empty;
        
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> Reject(RejectionComment rejectionComment)
    {
        if (RequestStatus is RequestStatus.Approved or RequestStatus.Rejected or RequestStatus.New)
            return Errors.VolunteerRequests.InvalidStatus();
        
        RequestStatus = RequestStatus.Rejected;
        RejectionComment = rejectionComment;
        
        return Result.Success<Error>();
    }
}