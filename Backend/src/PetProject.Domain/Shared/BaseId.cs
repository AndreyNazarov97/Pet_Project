namespace PetProject.Domain.Shared;

public class BaseId<TId> : ValueObject
    where TId : notnull
{
    public Guid Id { get; }

    protected BaseId(Guid id)
    {
        Id = id;
    }
    
    public static TId Create(Guid id) => (TId)Activator.CreateInstance(typeof(TId), id)!;
    public static TId NewId() => Create(Guid.NewGuid());
    public static TId Empty() => Create(Guid.Empty);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}