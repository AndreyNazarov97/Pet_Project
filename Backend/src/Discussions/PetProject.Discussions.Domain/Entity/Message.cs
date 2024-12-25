using CSharpFunctionalExtensions;
using PetProject.Discussions.Domain.ValueObjects;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Domain.Entity;

public class Message : SharedKernel.Shared.Common.Entity<MessageId>
{
    private Message(MessageId id) : base(id){}
    
    public Text Text { get; private set; }
    public long UserId { get; private set; }
    public bool IsEdited { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private Message(
        MessageId id,
        Text text,
        long userId) : base(id)
    {
        Text = text;
        UserId = userId;
        IsEdited = false;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Result<Message, Error> Create(Text text, long userId)
    {
        if (userId <= 0)
            return Errors.General.ValueIsInvalid(nameof(userId));
        
        var id = MessageId.NewId();

        return new Message(id, text, userId);
    }
    
    public void Edit(Text text)
    {
        Text = text;
        IsEdited = true;
    }

}