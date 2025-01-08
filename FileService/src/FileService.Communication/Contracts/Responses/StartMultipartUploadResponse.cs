namespace FileService.Communication.Contracts.Responses;

public record StartMultipartUploadResponse()
{
    public string Key { get; init; } = null!;
    public string UploadId { get; init; } = null!;
}