namespace PetProject.Domain.Entities;
/// <summary>
/// Порода 
/// </summary>
public class Breed : Entity
{
    public string Name { get; private set; }
    
    public Guid SpeciesId { get; private set; }

    private Breed()
    {
        
    }
}