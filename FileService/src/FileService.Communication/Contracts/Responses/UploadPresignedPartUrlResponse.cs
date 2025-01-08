namespace FileService.Communication.Contracts.Responses;

public record UploadPresignedPartUrlResponse
{
    public string Key { get; init; } = null!;
    public string Url { get; init; } = null!;
}