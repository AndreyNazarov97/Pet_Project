namespace PetProject.Domain.Shared.EntityIds;

public class VolunteerId 
{
    private VolunteerId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());
    public static VolunteerId Empty() => new(Guid.Empty);
    
    public static implicit operator Guid(VolunteerId id) => id.Id;

    public override string ToString() => Id.ToString();
}