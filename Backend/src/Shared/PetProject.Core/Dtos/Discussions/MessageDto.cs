namespace PetProject.Core.Dtos.Discussions;

public class MessageDto
{
    public required Guid Id { get; set; }
    public required string Text { get; set; }
    public required long UserId { get; set; }
    public required bool IsEdited { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}