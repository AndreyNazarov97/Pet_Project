using Amazon.S3.Model;
using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Models;

namespace FileService.Infrastructure.Providers;

public interface IFileProvider
{
    Task<InitiateMultipartUploadResponse> StartMultipartUpload(FileMetadata fileMetadata,
        CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetPresignedUrlPart(
        FileMetadata fileMetadata, CancellationToken cancellationToken);

    Task<CompleteMultipartUploadResponse> CompleteMultipartUpload(FileMetadata fileMetadata,
        CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetPresignedUrlForUpload(
        FileMetadata fileMetadata,
        CancellationToken cancellationToken);

    Task<Result<string, ErrorList>> GetPresignedUrlForDownload(FileMetadata fileMetadata,
        CancellationToken cancellationToken);

    Task<Result<FileMetadata, ErrorList>> GetObjectMetadata(string bucketName, string key,
        CancellationToken cancellationToken = default);

    public Task<Result<IReadOnlyCollection<string>, ErrorList>> DownloadFiles(
        IEnumerable<FileMetadata> filesData, CancellationToken cancellationToken = default);

    public Task<UnitResult<ErrorList>> DeleteFile(
        FileMetadata fileMetadata, CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetPresignedUrlForDelete(
        FileMetadata fileMetadata,
        CancellationToken cancellationToken = default);
}