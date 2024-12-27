using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos.VolunteerRequests;

namespace VolunteerRequests.Application.RequestsManagement.Queries.GetRequestsByUserId;

public class GetRequestsByUserIdHandler : IRequestHandler<GetRequestsByUserIdQuery, VolunteerRequestDto[]>
{
    private readonly IReadRepository _readRepository;

    public GetRequestsByUserIdHandler(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }
    
    public async Task<VolunteerRequestDto[]> Handle(GetRequestsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var query = new VolunteerRequestQueryModel()
        {
            UserIds = [request.UserId],
            SortBy = request.SortBy,
            SortDescending = request.SortDescending,
            Offset = request.Offset,
            Limit = request.Limit
        };
        
        var requests = await _readRepository.QueryVolunteerRequests(query, cancellationToken);
        
        return requests;
    }
}