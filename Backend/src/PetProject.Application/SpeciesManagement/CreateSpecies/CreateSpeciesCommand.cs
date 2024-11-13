namespace PetProject.Application.SpeciesManagement.CreateSpecies;

public record CreateSpeciesCommand
{
    public required string Name { get; init; }
}