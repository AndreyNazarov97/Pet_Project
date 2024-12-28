using VolunteerRequests.Application.RequestsManagement.Commands.Approve;
using VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;
using VolunteerRequests.Application.RequestsManagement.Commands.Reject;
using VolunteerRequests.Application.RequestsManagement.Commands.SendForRevision;
using VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;
using VolunteerRequests.Application.RequestsManagement.Commands.UpdateVolunteerRequest;
using VolunteerRequests.Application.RequestsManagement.Queries.GetNewRequests;
using VolunteerRequests.Application.RequestsManagement.Queries.GetRequestsByAdminId;
using VolunteerRequests.Application.RequestsManagement.Queries.GetRequestsByUserId;
using VolunteerRequests.Contracts.Requests;

namespace VolunteerRequests.Application.Mappers;

public static class RequestMappers
{
    public static CreateVolunteerRequestCommand ToCommand(this CreateVolunteerRequestRequest request)
    {
        return new CreateVolunteerRequestCommand
        {
            UserId = request.UserId,
            FullName = request.FullNameDto,
            Description = request.VolunteerDescription,
            AgeExperience = request.WorkExperience,
            PhoneNumber = request.PhoneNumber,
            SocialNetworksDto = request.SocialNetworksDto
        };
    }

    public static SendForRevisionCommand ToCommand(this SendForRevisionRequest request, Guid volunteerRequestId)
    {
        return new SendForRevisionCommand
        {
            VolunteerRequestId = volunteerRequestId,
            AdminId = request.AdminId,
            RejectionComment = request.RejectComment
        };
    }

    public static ApproveVolunteerRequestCommand ToCommand(this ApproveVolunteerRequestRequest request,
        Guid volunteerRequestId)
    {
        return new ApproveVolunteerRequestCommand
        {
            VolunteerRequestId = volunteerRequestId,
            AdminId = request.AdminId
        };
    }
    
    public static RejectVolunteerRequestCommand ToCommand(this RejectVolunteerRequestRequest request,
        Guid volunteerRequestId)
    {
        return new RejectVolunteerRequestCommand
        {
            VolunteerRequestId = volunteerRequestId,
            AdminId = request.AdminId,
            RejectionComment = request.RejectComment
        };
    }

    public static TakeVolunteerRequestOnReviewCommand ToCommand(this TakeVolunteerRequestOnReviewRequest request,
        Guid volunteerRequestId)
    {
        return new TakeVolunteerRequestOnReviewCommand
        {
            VolunteerRequestId = volunteerRequestId,
            AdminId = request.AdminId
        };
    }
    
    public static UpdateVolunteerRequestCommand ToCommand(this UpdateVolunteerRequestRequest request,
        Guid volunteerRequestId)
    {
        return new UpdateVolunteerRequestCommand
        {
            VolunteerRequestId = volunteerRequestId,
            UserId = request.UserId,
            FullName = request.FullNameDto,
            Description = request.VolunteerDescription,
            WorkExperience = request.WorkExperience,
            PhoneNumber = request.PhoneNumber,
            SocialNetworksDto = request.SocialNetworksDto,

        };
    }

    public static GetNewRequestsQuery ToQuery(this GetNewVolunteerRequestsRequest request)
    {
        return new GetNewRequestsQuery
        {
            SortBy = request.SortBy,
            SortDescending = request.SortDescending ?? false,
            Offset = request.Offset,
            Limit = request.Limit
        };
    }

    public static GetRequestsByAdminIdQuery ToQuery(this GetVolunteerRequestsByAdminIdRequest request)
    {
        return new GetRequestsByAdminIdQuery
        {
            AdminId = request.AdminId,
            SortBy = request.SortBy,
            SortDescending = request.SortDescending ?? false,
            Offset = request.Offset,
            Limit = request.Limit
        };
    }
    
    public static GetRequestsByUserIdQuery ToQuery(this GetVolunteerRequestsByUserIdRequest request)
    {
        return new GetRequestsByUserIdQuery
        {
            UserId = request.UserId,
            SortBy = request.SortBy,
            SortDescending = request.SortDescending ?? false,
            Offset = request.Offset,
            Limit = request.Limit
        };
    }
}