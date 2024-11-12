namespace PetProject.Application.SpeciesManagement.CreateBreed;

public record CreateBreedRequest
{
    public string SpeciesName { get; init; }
    public string BreedName { get; init; }
}