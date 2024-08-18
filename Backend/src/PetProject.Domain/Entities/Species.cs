using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

/// <summary>
/// Вид Животного
/// </summary>
public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    private Species(){}
    private Species(SpeciesId id, string name)
    : base(id)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
    public IReadOnlyCollection<Breed> Breeds => _breeds;

    public void AddBreeds(List<Breed> breeds) => _breeds.AddRange(breeds);

    public static Result<Species> Create(SpeciesId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(name));
                
        return new Species(id, name);
    }

}