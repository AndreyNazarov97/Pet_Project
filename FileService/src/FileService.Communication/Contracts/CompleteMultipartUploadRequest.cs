namespace FileService.Communication.Contracts;

public record CompleteMultipartUploadRequest(
    string UploadId,
    string BucketName,
    string ContentType,
    string Prefix,
    string FileName,
    List<PartETagInfo> Parts);
    
public record PartETagInfo(int PartNumber, string ETag);