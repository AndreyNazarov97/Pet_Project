namespace FileService.Communication.Contracts.Responses;

public record CompleteMultipartUploadResponse
{
    public required Guid FileId { get; init; }
    public required string Location { get; init; }
}