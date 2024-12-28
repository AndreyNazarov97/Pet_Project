namespace VolunteerRequests.Contracts.Requests;

public record GetNewVolunteerRequestsRequest(string? SortBy, bool? SortDescending, int Limit, int Offset);