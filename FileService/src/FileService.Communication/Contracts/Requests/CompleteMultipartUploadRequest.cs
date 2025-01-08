namespace FileService.Communication.Contracts.Requests;

public record CompleteMultipartUploadRequest(
    string UploadId,
    string BucketName,
    string FileName,
    IEnumerable<PartETagInfo> Parts);
    
public record PartETagInfo(int PartNumber, string ETag);