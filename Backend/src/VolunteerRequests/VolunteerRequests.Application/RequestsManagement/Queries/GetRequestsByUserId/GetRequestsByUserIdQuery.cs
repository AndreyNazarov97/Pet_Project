using MediatR;
using PetProject.Core.Dtos.VolunteerRequests;

namespace VolunteerRequests.Application.RequestsManagement.Queries.GetRequestsByUserId;

public record GetRequestsByUserIdQuery : IRequest<VolunteerRequestDto[]>
{
    public long UserId { get; init; }
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; }
    public int Offset { get; init; } 
    public int Limit { get; init; } 
}