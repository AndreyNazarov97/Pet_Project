namespace PetProject.SharedKernel.Shared.Common;

public class SoftDeletableEntity<TId> : Entity<TId> where TId : notnull
{
    protected SoftDeletableEntity(TId id) : base(id)
    {
    }
    
    public bool IsDeleted { get; protected set; }
    public DateTimeOffset? DeletionDate { get; protected set; }    
    
    public virtual void Delete(DateTimeOffset deletionDate)
    {
        IsDeleted = true;
        DeletionDate = deletionDate;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
        DeletionDate = null;
    }
}