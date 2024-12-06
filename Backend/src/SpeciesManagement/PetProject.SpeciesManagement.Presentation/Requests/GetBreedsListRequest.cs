using PetProject.SpeciesManagement.Application.SpeciesManagement.Queries.GetBreedsList;

namespace PetProject.SpeciesManagement.Presentation.Requests;

public record GetBreedsListRequest( int PageNumber, int PageSize)
{
    public GetBreedsListQuery ToQuery(string speciesName) => new()
    {
        SpeciesName = speciesName,
        Offset = (PageNumber - 1) * PageSize,
        Limit = PageSize
    };
}