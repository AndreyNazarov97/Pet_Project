namespace FileService.Communication.Contracts.Requests;

public record StartMultipartUploadRequest(
    string BucketName,
    string FileName,
    string ContentType);