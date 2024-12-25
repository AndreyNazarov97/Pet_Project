namespace PetProject.SharedKernel.Shared.EntityIds;

public record MessageId
{
    public Guid Id { get; }

    private MessageId(Guid id) => Id = id;

    public static MessageId NewId() => new (Guid.NewGuid());
    public static MessageId Empty() => new (Guid.Empty);
    public static MessageId Create(Guid id) => new (id);
    public static implicit operator Guid(MessageId messageId) => messageId.Id;
}