namespace FileService.Communication.Contracts;

public record StartMultipartUploadRequest(
    string BucketName,
    string FileName,
    string ContentType,
    string Prefix);