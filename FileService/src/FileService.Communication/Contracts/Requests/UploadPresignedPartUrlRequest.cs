namespace FileService.Communication.Contracts.Requests;

public record UploadPresignedPartUrlRequest(
    string UploadId,
    int PartNumber,
    string BucketName);