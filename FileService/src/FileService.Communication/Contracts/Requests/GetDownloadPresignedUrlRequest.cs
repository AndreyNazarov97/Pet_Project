namespace FileService.Communication.Contracts.Requests;

public record GetDownloadPresignedUrlRequest(
    string BucketName,
    string Prefix);