namespace VolunteerRequests.Contracts.Requests;

public record SendForRevisionRequest(long AdminId, string RejectComment);
