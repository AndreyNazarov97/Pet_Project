namespace PetProject.Discussions.Contracts.Requests;

public record UpdateMessageRequest(Guid DiscussionId, Guid MessageId, long UserId, string Text)
{
    
}