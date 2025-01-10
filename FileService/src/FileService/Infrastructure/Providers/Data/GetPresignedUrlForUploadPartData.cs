namespace FileService.Infrastructure.Providers.Data;

public record GetPresignedUrlForUploadPartData(string BucketName, string Key, string UploadId, int PartNumber);