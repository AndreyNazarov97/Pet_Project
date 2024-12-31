namespace PetProject.Discussions.Contracts.Requests;

public record DeleteMessageRequest(Guid DiscussionId, Guid MessageId, long UserId);