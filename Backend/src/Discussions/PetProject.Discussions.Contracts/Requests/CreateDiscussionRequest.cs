namespace PetProject.Discussions.Contracts.Requests;

public record CreateDiscussionRequest(
    Guid RelationId, long FirstMemberId, long SecondMemberId);