using PetProject.Application.SpeciesManagement.GetSpeciesList;

namespace PetProject.API.Controllers.Species.Requests;

public record GetSpeciesListRequest(int PageNumber, int PageSize)
{
    public GetSpeciesListQuery ToQuery() => new()
    {
        Offset = (PageNumber - 1) * PageSize,
        Limit = PageSize
    };
}