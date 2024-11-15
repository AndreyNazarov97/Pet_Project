using PetProject.Application.SpeciesManagement.CreateBreed;

namespace PetProject.API.Controllers.Species.Requests;

public record CreateBreedRequest(string Name)
{
    public CreateBreedCommand ToCommand(string speciesName) => new()
    {
        SpeciesName = speciesName,
        BreedName = Name
    };
}
