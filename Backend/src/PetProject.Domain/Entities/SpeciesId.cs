namespace PetProject.Domain.Entities;

public class SpeciesId 
{
    private SpeciesId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static SpeciesId NewSpeciesId() => new(Guid.NewGuid());
    public static SpeciesId Empty() => new(Guid.Empty);
}