using PetProject.Application.VolunteersManagement.GetVolunteersList;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record GetVolunteersListRequest(int PageNumber, int PageSize)
{
    public GetVolunteersListQuery ToQuery() => new()
    {
        Offset = (PageNumber - 1) * PageSize,
        Limit = PageSize
    };
}