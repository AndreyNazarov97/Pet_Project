using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;
/// <summary>
/// Порода 
/// </summary>
public class Breed : Entity<BreedId>
{
    private Breed(){}

    private Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }
    
    public string Name { get; private set; }

    public static Result<Breed> Create(BreedId breedId ,string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Result<Breed>.Failure(new("Invalid name", $"{nameof(name)} cannot be null or empty or longer than {Constants.MAX_SHORT_TEXT_LENGTH} characters."));
        
        var breed = new Breed(breedId, name);
        return Result<Breed>.Success(breed);
    }
    
}