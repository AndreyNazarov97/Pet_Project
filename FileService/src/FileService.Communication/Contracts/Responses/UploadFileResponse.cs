namespace FileService.Communication.Contracts.Responses;

public record UploadFileResponse
{
    public string Key { get; init; } = null!;
    public required Guid FileId { get; init; }
}