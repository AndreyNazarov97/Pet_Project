using Microsoft.AspNetCore.Http;

namespace FileService.Communication.Contracts.Requests;

public record UploadFileRequest
{
    public required string PresignedUrl { get; init; }
    
    public required IFormFile File { get; set; } 
}