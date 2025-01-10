namespace FileService.Infrastructure.Providers.Data;

public record UploadFileData(Stream Stream, string BucketName, string Key, string ContentType);