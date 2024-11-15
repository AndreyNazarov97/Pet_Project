using PetProject.Application.SpeciesManagement.CreateSpecies;

namespace PetProject.API.Controllers.Species.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand() => new()
    {
        Name = Name
    };
}