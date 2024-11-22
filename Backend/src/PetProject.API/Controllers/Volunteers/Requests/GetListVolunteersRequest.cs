using PetProject.Application.VolunteersManagement.GetListVolunteers;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record GetListVolunteersRequest(int PageNumber, int PageSize)
{
    public GetListVolunteersQuery ToQuery() => new()
    {
        Offset = (PageNumber - 1) * PageSize,
        Limit = PageSize
    };
}