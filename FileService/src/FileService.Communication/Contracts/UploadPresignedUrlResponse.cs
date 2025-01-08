namespace FileService.Communication.Contracts;

public record UploadPresignedUrlResponse
{
    public string Key { get; init; } = null!;
    public string Url { get; init; } = null!;
}