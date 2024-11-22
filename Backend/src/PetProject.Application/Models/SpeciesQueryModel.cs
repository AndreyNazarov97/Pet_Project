namespace PetProject.Application.Models;

public class SpeciesQueryModel
{
    public Guid[] SpeciesIds { get; init; } = [];
    public Guid[] BreedIds { get; init; } = [];
    public int Limit { get; init; } 
    public int Offset { get; init; }
    
    public bool IsEmpty()
    {
        return SpeciesIds is not { Length: > 0 } &&
               BreedIds is not { Length: > 0 };
    }
}
