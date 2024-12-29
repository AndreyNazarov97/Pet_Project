namespace PetProject.SharedKernel.Shared.EntityIds;

public record MemberId
{
    public Guid Id { get; }

    private MemberId(Guid id) => Id = id;

    public static MemberId NewId() => new (Guid.NewGuid());
    public static MemberId Empty() => new (Guid.Empty);
    public static MemberId Create(Guid id) => new (id);
    public static implicit operator Guid(MemberId memberId) => memberId.Id;
}