namespace PetProject.SharedKernel.Shared.EntityIds;

public class UserRestrictionId
{
    public Guid Id { get; }

    private UserRestrictionId(Guid id) => Id = id;

    public static UserRestrictionId NewId() => new (Guid.NewGuid());
    public static UserRestrictionId Empty() => new (Guid.Empty);
    public static UserRestrictionId Create(Guid id) => new (id);
    public static implicit operator Guid(UserRestrictionId userRestrictionId) => userRestrictionId.Id;
}