namespace PetProject.Discussions.Contracts.Requests;

public record SendMessageRequest(Guid DiscussionId, long UserId, string Text)
{
    
}