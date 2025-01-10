namespace FileService.Communication.Contracts.Responses;

public record UploadPresignedUrlResponse
{
    public required Guid FileId { get; init; }
    public string Url { get; init; } = null!;
}