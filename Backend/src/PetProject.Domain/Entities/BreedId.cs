namespace PetProject.Domain.Entities;

public class BreedId 
{
    private BreedId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static BreedId NewBreedId() => new(Guid.NewGuid());
    public static BreedId Empty() => new(Guid.Empty);
    
    public override string ToString() => Id.ToString();

}