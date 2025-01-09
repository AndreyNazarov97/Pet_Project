using Microsoft.AspNetCore.Http;

namespace FileService.Communication.Contracts.Requests;

public record UploadFileRequest
{
    public required string BucketName { get; init; }
    
}