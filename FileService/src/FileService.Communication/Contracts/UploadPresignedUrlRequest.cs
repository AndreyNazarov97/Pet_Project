namespace FileService.Communication.Contracts;

public record UploadPresignedUrlRequest(
    string BucketName,
    string FileName, 
    string ContentType,
    string Prefix);