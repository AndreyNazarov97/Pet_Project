using MediatR;
using PetProject.Core.Dtos.VolunteerRequests;

namespace VolunteerRequests.Application.RequestsManagement.Queries.GetRequestsByAdminId;

public record GetRequestsByAdminIdQuery : IRequest<VolunteerRequestDto[]>
{
    public long AdminId { get; init; }
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; }
    public int Offset { get; init; } 
    public int Limit { get; init; } 
}