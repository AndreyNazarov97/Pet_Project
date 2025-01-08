namespace FileService.Infrastructure.Providers.Data;

public record StartMultipartUploadData(string BucketName, string FileName, string Key, string ContentType);