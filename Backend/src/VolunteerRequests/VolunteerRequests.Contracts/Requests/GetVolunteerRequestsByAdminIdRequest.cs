namespace VolunteerRequests.Contracts.Requests;

public record GetVolunteerRequestsByAdminIdRequest(
    long AdminId,
    string? SortBy,
    bool? SortDescending,
    int Limit,
    int Offset);