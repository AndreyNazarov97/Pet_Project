namespace PetProject.Domain.Entities;

/// <summary>
/// Вид Животного
/// </summary>
public class Species : Entity
{
    public string Name { get; private set; }

    public List<Breed> Breeds { get; private set; } = [];

    private Species()
    {
        
    }
}