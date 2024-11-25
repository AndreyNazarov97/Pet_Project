namespace PetProject.Application.Models;

public class SpeciesQueryModel
{
    public string SpeciesName { get; init; } = string.Empty;
    public string BreedName { get; init; } = string.Empty;
    public Guid[] SpeciesIds { get; init; } = [];
    public Guid[] BreedIds { get; init; } = [];
    public int Limit { get; init; } 
    public int Offset { get; init; }
}
