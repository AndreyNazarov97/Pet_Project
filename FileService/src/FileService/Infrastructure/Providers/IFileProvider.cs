using Amazon.S3.Model;
using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Models;
using FileService.Infrastructure.Providers.Data;

namespace FileService.Infrastructure.Providers;

public interface IFileProvider
{
    Task<UnitResult<ErrorList>> UploadFiles(
        IEnumerable<UploadFileData> files, CancellationToken cancellationToken = default);
    
    Task<InitiateMultipartUploadResponse> StartMultipartUpload(
        StartMultipartUploadData data, CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetPresignedUrlPart(
        GetPresignedUrlForUploadPartData data, CancellationToken cancellationToken);

    Task<CompleteMultipartUploadResponse> CompleteMultipartUpload(
        CompleteMultipartUploadData data, CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetPresignedUrlForUpload(
        GetPresignedUrlForUploadData data,
        CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetPresignedUrlForDownload(FileMetadata fileMetadata,
        CancellationToken cancellationToken = default);

    Task<GetObjectMetadataResponse>  GetObjectMetadata(string bucketName, string key,
        CancellationToken cancellationToken = default);

    public Task<Result<IReadOnlyCollection<string>, ErrorList>> DownloadFiles(
        IEnumerable<FileMetadata> filesData, CancellationToken cancellationToken = default);
    
    Task<UnitResult<ErrorList>> DeleteFile(
        string bucketName, string key, CancellationToken cancellationToken = default);

    Task<UnitResult<ErrorList>> DeleteFiles(
        IEnumerable<FileMetadata> files, CancellationToken cancellationToken = default);
}