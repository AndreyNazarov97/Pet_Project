namespace VolunteerRequests.Contracts.Requests;

public record GetVolunteerRequestsByUserIdRequest(
    long UserId,
    string? SortBy,
    bool? SortDescending,
    int Limit,
    int Offset);