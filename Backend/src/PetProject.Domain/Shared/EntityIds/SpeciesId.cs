namespace PetProject.Domain.Shared.EntityIds;

public class SpeciesId 
{
    private SpeciesId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static SpeciesId NewSpeciesId() => new(Guid.NewGuid());
    public static SpeciesId FromGuid(Guid id) => new(id);
    public static SpeciesId Empty() => new(Guid.Empty);
    
    public override string ToString() => Id.ToString();
}