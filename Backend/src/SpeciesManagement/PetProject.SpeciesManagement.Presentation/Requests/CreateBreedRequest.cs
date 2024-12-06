using PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.CreateBreed;

namespace PetProject.SpeciesManagement.Presentation.Requests;

public record CreateBreedRequest(string Name)
{
    public CreateBreedCommand ToCommand(string speciesName) => new()
    {
        SpeciesName = speciesName,
        BreedName = Name
    };
}
