namespace PetProject.Domain.Entities;

public class PetPhoto
{
    public Guid Id { get; }
    
    public string Path { get; }
    
    public bool IsMain { get; }
}