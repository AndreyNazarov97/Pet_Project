using MediatR;
using PetProject.Core.Dtos.VolunteerRequests;

namespace VolunteerRequests.Application.RequestsManagement.Queries.GetNewRequests;

public record GetNewRequestsQuery : IRequest<VolunteerRequestDto[]>
{
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; }
    public int Offset { get; init; } 
    public int Limit { get; init; } 
}