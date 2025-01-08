namespace FileService.Communication.Contracts.Requests;

public record UploadPresignedUrlRequest(
    string BucketName,
    string FileName, 
    string ContentType);