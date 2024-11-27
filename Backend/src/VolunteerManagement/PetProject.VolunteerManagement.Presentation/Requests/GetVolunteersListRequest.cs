using PetProject.VolunteerManagement.Application.VolunteersManagement.GetVolunteersList;

namespace PetProject.VolunteerManagement.Presentation.Requests;

public record GetVolunteersListRequest(int PageNumber, int PageSize)
{
    public GetVolunteersListQuery ToQuery() => new()
    {
        Offset = (PageNumber - 1) * PageSize,
        Limit = PageSize
    };
}