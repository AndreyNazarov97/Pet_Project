namespace PetProject.Domain.Entities;

public class Requisite
{
    public Guid Id { get; }
    public string Title { get; }
    public string Description { get; }

    private Requisite()
    {
        
    }
}