namespace PetProject.Core.Dtos;

public record FileMetaDataDto()
{
    public required string ObjectName { get; init; } 
    public required string BucketName { get; init; } 
}