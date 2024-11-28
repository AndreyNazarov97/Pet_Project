namespace PetProject.Core.Dtos;

public record SocialLinkDto()
{
    public required string Title { get; init; } 
    public required string Url { get; init; } 
}