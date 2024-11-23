using PetProject.Application.SpeciesManagement.DeleteSpecies;

namespace PetProject.API.Controllers.Species.Requests;

public record DeleteSpeciesRequest(string Name)
{
    public DeleteSpeciesCommand ToCommand() => new()
    {
        SpeciesName = Name
    };
}