namespace PetProject.Domain.Shared.Common;

public class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    public AggregateRoot(TId id) : base(id)
    {
    }
}