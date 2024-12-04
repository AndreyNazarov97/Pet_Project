namespace PetProject.Core.Dtos;

public record SocialNetworkDto()
{
    public required string Title { get; init; } 
    public required string Url { get; init; } 
}