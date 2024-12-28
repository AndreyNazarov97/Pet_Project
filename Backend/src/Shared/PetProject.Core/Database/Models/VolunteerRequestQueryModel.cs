namespace PetProject.Core.Database.Models;

public class VolunteerRequestQueryModel
{
    public Guid[] VolunteerRequestIds { get; init; } = [];
    public long[] AdminIds { get; init; } = [];
    public long[] UserIds { get; init; } = [];
    public string? RequestStatus { get; init; }
    public string? SortBy { get; init; } 
    public bool SortDescending { get; init; } 
    public int Limit { get; init; } 
    public int Offset { get; init; }
}