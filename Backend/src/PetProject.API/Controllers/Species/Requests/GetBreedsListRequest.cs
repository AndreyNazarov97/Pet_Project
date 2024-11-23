using PetProject.Application.SpeciesManagement.GetBreedsList;

namespace PetProject.API.Controllers.Species.Requests;

public record GetBreedsListRequest( int PageNumber, int PageSize)
{
    public GetBreedsListQuery ToQuery(string speciesName) => new()
    {
        SpeciesName = speciesName,
        Offset = (PageNumber - 1) * PageSize,
        Limit = PageSize
    };
}