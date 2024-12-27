using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos.VolunteerRequests;
using VolunteerRequests.Domain.Enums;

namespace VolunteerRequests.Application.RequestsManagement.Queries.GetNewRequests;

public class GetNewRequestsHandler : IRequestHandler<GetNewRequestsQuery, VolunteerRequestDto[]>
{
    private readonly IReadRepository _readRepository;

    public GetNewRequestsHandler(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }
    
    public async Task<VolunteerRequestDto[]> Handle(GetNewRequestsQuery request, CancellationToken cancellationToken)
    {
        
        var query = new VolunteerRequestQueryModel()
        {
            SortBy = request.SortBy,
            SortDescending = request.SortDescending,
            RequestStatus = RequestStatus.New.ToString(),
            Offset = request.Offset,
            Limit = request.Limit
        };
       
        var requests = await _readRepository.QueryVolunteerRequests(query, cancellationToken);
        
        return requests;
    }
}