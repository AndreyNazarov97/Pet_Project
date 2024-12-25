namespace PetProject.SharedKernel.Shared.EntityIds;

public record DiscussionId
{
    public Guid Id { get; }

    private DiscussionId(Guid id) => Id = id;

    public static DiscussionId NewId() => new (Guid.NewGuid());
    public static DiscussionId Empty() => new (Guid.Empty);
    public static DiscussionId Create(Guid id) => new (id);
    public static implicit operator Guid(DiscussionId id) => id.Id;
}