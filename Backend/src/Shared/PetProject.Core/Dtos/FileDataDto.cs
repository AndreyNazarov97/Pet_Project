namespace PetProject.Core.Dtos;

public record FileDataDto()
{
    public required Stream Stream { get; init; } 
    public required string ObjectName { get; init; } 
    public required string BucketName { get; init; } 
}

