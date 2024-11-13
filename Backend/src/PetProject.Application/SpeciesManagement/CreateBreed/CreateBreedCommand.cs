namespace PetProject.Application.SpeciesManagement.CreateBreed;

public record CreateBreedCommand
{
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
}