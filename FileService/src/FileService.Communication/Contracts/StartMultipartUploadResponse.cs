namespace FileService.Communication.Contracts;

public record StartMultipartUploadResponse
{
    public string? Key { get; init; }
    public string? UploadId { get; init; }
    public string? BucketName { get; init; }
    public string? Prefix { get; init; }
}