using PetProject.SpeciesManagement.Application.SpeciesManagement.CreateBreed;

namespace PetProject.SpeciesManagement.Presentation.Requests;

public record CreateBreedRequest(string Name)
{
    public CreateBreedCommand ToCommand(string speciesName) => new()
    {
        SpeciesName = speciesName,
        BreedName = Name
    };
}
