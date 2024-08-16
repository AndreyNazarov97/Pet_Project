namespace PetProject.Domain.Entities;

public class VolunteerId 
{
    private VolunteerId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());
    public static VolunteerId Empty() => new(Guid.Empty);

    public override string ToString() => Id.ToString();
}