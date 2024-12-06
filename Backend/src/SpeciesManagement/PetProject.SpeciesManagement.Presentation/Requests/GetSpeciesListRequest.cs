using PetProject.SpeciesManagement.Application.SpeciesManagement.Queries.GetSpeciesList;

namespace PetProject.SpeciesManagement.Presentation.Requests;

public record GetSpeciesListRequest(int PageNumber, int PageSize)
{
    public GetSpeciesListQuery ToQuery() => new()
    {
        Offset = (PageNumber - 1) * PageSize,
        Limit = PageSize
    };
}