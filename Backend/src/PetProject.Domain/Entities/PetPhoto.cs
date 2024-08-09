namespace PetProject.Domain.Entities;

public class PetPhoto
{
    public string Path { get; private set; }
    
    public bool IsMain { get; private set; }

    private PetPhoto()
    {
    }
}