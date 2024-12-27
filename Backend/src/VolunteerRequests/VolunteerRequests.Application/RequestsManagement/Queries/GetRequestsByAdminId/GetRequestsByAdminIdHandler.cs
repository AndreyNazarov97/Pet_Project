using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos.VolunteerRequests;

namespace VolunteerRequests.Application.RequestsManagement.Queries.GetRequestsByAdminId;

public class GetRequestsByAdminIdHandler : IRequestHandler<GetRequestsByAdminIdQuery, VolunteerRequestDto[]>
{
    private readonly IReadRepository _readRepository;

    public GetRequestsByAdminIdHandler(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }
    
    public async Task<VolunteerRequestDto[]> Handle(GetRequestsByAdminIdQuery request, CancellationToken cancellationToken)
    {
        var query = new VolunteerRequestQueryModel()
        {
            AdminIds = [request.AdminId],
            SortBy = request.SortBy,
            SortDescending = request.SortDescending,
            Offset = request.Offset,
            Limit = request.Limit
        };
        
        var requests = await _readRepository.QueryVolunteerRequests(query, cancellationToken);
        
        return requests;
    }
}