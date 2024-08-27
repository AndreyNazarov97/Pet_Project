namespace PetProject.Domain.Shared.EntityIds;

public class BreedId 
{
    private BreedId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static BreedId NewBreedId() => new(Guid.NewGuid());
    public static BreedId FromGuid(Guid id) => new(id);
    public static BreedId Empty() => new(Guid.Empty);
    
    public override string ToString() => Id.ToString();

}