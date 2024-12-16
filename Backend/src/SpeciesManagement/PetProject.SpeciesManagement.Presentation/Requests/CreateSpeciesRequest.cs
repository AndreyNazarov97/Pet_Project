using PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.CreateSpecies;

namespace PetProject.SpeciesManagement.Presentation.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand() => new()
    {
        Name = Name
    };
}