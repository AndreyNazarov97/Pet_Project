using CSharpFunctionalExtensions;
using PetProject.Discussions.Domain.Entity;
using PetProject.Discussions.Domain.Enums;
using PetProject.Discussions.Domain.ValueObjects;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.Common;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Domain.Aggregate;

public class Discussion : AggregateRoot<DiscussionId>
{
    private readonly List<Message> _messages = [];

    private Discussion(DiscussionId id) : base(id){}
    
    public Guid RelationId { get; private set; }
    public Members Members { get; private set; }
    public DiscussionStatus Status { get; private set; }
    public IReadOnlyList<Message> Messages => _messages;

    private Discussion(
        DiscussionId id,
        DiscussionStatus status,
        Guid relationId,
        Members members) : base(id)
    {
        Status = status;
        RelationId = relationId;
        Members = members;
    }

    public static Result<Discussion, Error> Create(Guid relationId, Members members)
    {
        if (relationId == Guid.Empty)
            return Errors.General.ValueIsInvalid(nameof(relationId));

        var id = DiscussionId.NewId();
        var status = DiscussionStatus.Opened;

        return new Discussion(id, status, relationId, members);
    }

    public UnitResult<Error> AddMessage(Message message)
    {
        if (message.UserId != Members.FirstMemberId && message.UserId != Members.SecondMemberId)
            return Errors.Discussion.UserNotInDiscussion(message.UserId);

        _messages.Add(message);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> DeleteMessage(long userId, MessageId messageId)
    {
        var message = _messages.FirstOrDefault(m => m.Id == messageId);
        if (message == null)
            return Errors.General.NotFound(messageId.Id);

        if (message.UserId != userId)
            return Errors.Discussion.NotYourMessage();

        _messages.Remove(message);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> EditMessage(long userId, MessageId messageId, Text text)
    {
        var message = _messages.FirstOrDefault(m => m.Id == messageId);
        if (message == null)
            return Errors.General.NotFound(messageId.Id);

        if (message.UserId != userId)
            return Errors.Discussion.NotYourMessage();

        message.Edit(text);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> CloseDiscussion(long userId)
    {
        if(Status == DiscussionStatus.Closed)
            return Errors.Discussion.DiscussionAlreadyClosed();
        
        if (userId != Members.FirstMemberId && userId != Members.SecondMemberId)
            return Errors.Discussion.UserNotInDiscussion(userId);
        
        Status = DiscussionStatus.Closed;
        
        return UnitResult.Success<Error>();
    }

}