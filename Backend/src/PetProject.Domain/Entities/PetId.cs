namespace PetProject.Domain.Entities;

public class PetId 
{
    private PetId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static PetId NewPetId() => new(Guid.NewGuid());
    public static PetId Empty() => new(Guid.Empty);
    
    public override string ToString() => Id.ToString();
}