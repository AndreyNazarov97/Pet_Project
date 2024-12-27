namespace VolunteerRequests.Contracts.Requests;

public record RejectVolunteerRequestRequest(long AdminId, string RejectComment);
