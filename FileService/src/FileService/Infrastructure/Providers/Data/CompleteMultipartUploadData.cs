using FileService.Communication.Contracts;
using FileService.Communication.Contracts.Requests;

namespace FileService.Infrastructure.Providers.Data;

public record CompleteMultipartUploadData(string BucketName, string UploadId, string Key, IEnumerable<PartETagInfo> ETags);