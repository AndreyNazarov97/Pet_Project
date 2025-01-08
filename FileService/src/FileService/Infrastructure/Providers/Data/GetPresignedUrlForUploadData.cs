namespace FileService.Infrastructure.Providers.Data;

public record GetPresignedUrlForUploadData(string BucketName, string FileName, string Key, string ContentType);