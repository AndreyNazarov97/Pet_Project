using PetProject.SpeciesManagement.Application.SpeciesManagement.DeleteSpecies;

namespace PetProject.SpeciesManagement.Presentation.Requests;

public record DeleteSpeciesRequest(string Name)
{
    public DeleteSpeciesCommand ToCommand() => new()
    {
        SpeciesName = Name
    };
}