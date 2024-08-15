namespace PetProject.Domain.Shared;

public abstract class Entity<TId> 
    where TId : notnull
{
    public TId Id { get; } 

    protected Entity(){}
    
    protected Entity(TId id) 
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if(obj is not Entity<TId> other)
            return false;
        
        if (!ReferenceEquals(this, other) || !EqualityComparer<TId>.Default.Equals(Id, other.Id))
            return false;

        return true;
    }

    public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
    
    public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
    {
        if(a is null && b is null)
            return true;
        
        if(a is null || b is null)
            return false;
        
        return a.Equals(b);
    }
    public static bool operator !=(Entity<TId>? a, Entity<TId>? b) => !Equals(a, b);

    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Id = {Id}";
    }
}