namespace FileService.Communication.Contracts;

public record GetDownloadPresignedUrlRequest(
    string BucketName,
    string Prefix);