namespace PetProject.Domain.Entities;

public class PetPhoto : Entity
{
    public string Path { get; private set; }
    
    public bool IsMain { get; private set; }

    private PetPhoto()
    {
    }
}