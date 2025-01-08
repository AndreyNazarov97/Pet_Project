namespace FileService.Communication.Contracts;

public record CompleteMultipartUploadResponse
{
    public required string Location { get; init; }
}