namespace FileService.Communication.Contracts;

public record DeletePresignedUrlRequest(
    string BucketName,
    string FileName,
    string Prefix,
    string ContentType);