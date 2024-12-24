namespace PetProject.SharedKernel.Shared.Common;

public class AggregateRoot<TId> : SoftDeletableEntity<TId> where TId : notnull
{
    public AggregateRoot(TId id) : base(id)
    {
    }
}