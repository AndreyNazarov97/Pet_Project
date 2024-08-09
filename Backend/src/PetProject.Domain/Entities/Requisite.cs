namespace PetProject.Domain.Entities;

public class Requisite
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }

    private Requisite()
    {
        
    }
}