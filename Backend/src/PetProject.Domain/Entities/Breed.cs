namespace PetProject.Domain.Entities;
/// <summary>
/// Порода 
/// </summary>
public class Breed : Entity<BreedId>
{
    public string Name { get; private set; }
    
    private Breed()
    {
        
    }
}