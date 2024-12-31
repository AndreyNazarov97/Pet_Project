namespace PetProject.Core.Dtos.Discussions;

public record DiscussionDto
{
    public required Guid Id { get; set; }
    public required Guid RelationId { get; set; }
    public required MembersDto Members { get; set; }
    public required string Status { get; set; }
    public List<MessageDto> Messages { get; set; } = [];
}