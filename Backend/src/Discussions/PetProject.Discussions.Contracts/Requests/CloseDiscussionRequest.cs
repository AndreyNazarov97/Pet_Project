namespace PetProject.Discussions.Contracts.Requests;

public record CloseDiscussionRequest(Guid DiscussionId, long UserId);