using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Models;
using FileService.Infrastructure.Providers.Data;

namespace FileService.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MaxDegreeOfParallelism = 10;
    private const int Expiry = 604800; // 7 days
    private readonly IAmazonS3 _client;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(
        IAmazonS3 client,
        ILogger<MinioProvider> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> UploadFiles(
        IEnumerable<UploadFileData> files, CancellationToken cancellationToken = default)
    {
        var fileList = files.ToList();
        var semaphoreSlim = new SemaphoreSlim(MaxDegreeOfParallelism);
        
        await EnsureBucketExistsAsync(fileList.Select(f => f.BucketName), cancellationToken);

        var tasks = fileList.Select(async uploadFileData =>
            await UploadFile(uploadFileData, semaphoreSlim, cancellationToken));
        var results = await Task.WhenAll(tasks);

        var errors = results
            .Where(r => r.IsFailure)
            .Select(r => r.Error).ToList();

        if (errors.Count > 0)
            return new ErrorList(errors);

        return Result.Success<ErrorList>();
    }

    public async Task<InitiateMultipartUploadResponse> StartMultipartUpload(
        StartMultipartUploadData data, CancellationToken cancellationToken = default)
    {
        await EnsureBucketExistsAsync([data.BucketName], cancellationToken);
        
        var presignedRequest = new InitiateMultipartUploadRequest
        {
            BucketName = data.BucketName,
            Key = data.Key,
            ContentType = data.ContentType,
            Metadata =
            {
                ["file-name"] = data.FileName
            }
        };

        var response = await _client
            .InitiateMultipartUploadAsync(presignedRequest, cancellationToken);

        
        return response;
    }

    public async Task<Result<string, ErrorList>> GetPresignedUrlPart(GetPresignedUrlForUploadPartData data,
        CancellationToken cancellationToken)
    {
        try
        {
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = data.BucketName,
                Key = data.Key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddSeconds(Expiry),
                Protocol = Protocol.HTTP,
                UploadId = data.UploadId,
                PartNumber = data.PartNumber,
            };

            var result = await _client.GetPreSignedURLAsync(presignedRequest);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with key {key} in bucket {bucket}",
                data.Key,
                data.BucketName);

            return Errors.Files.FailUpload().ToErrorList();
        }
    }

    public async Task<CompleteMultipartUploadResponse> CompleteMultipartUpload(CompleteMultipartUploadData data,
        CancellationToken cancellationToken = default)
    {
        var presignedRequest = new CompleteMultipartUploadRequest()
        {
            BucketName = data.BucketName,
            Key = data.Key,
            UploadId = data.UploadId,
            PartETags = data.ETags!.Select(e => new PartETag(e.PartNumber, e.ETag)).ToList()
        };

        var response = await _client
            .CompleteMultipartUploadAsync(presignedRequest, cancellationToken);

        return response;
    }

    public async Task<Result<string, ErrorList>> GetPresignedUrlForUpload(
        GetPresignedUrlForUploadData data,
        CancellationToken cancellationToken)
    {
        try
        {
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = data.BucketName,
                Key = data.Key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddSeconds(Expiry),
                ContentType = data.ContentType,
                Protocol = Protocol.HTTP,
                UploadId = Guid.NewGuid().ToString(),
                Metadata =
                {
                    ["file-name"] = data.FileName
                }
            };

            var result = await _client.GetPreSignedURLAsync(presignedRequest);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with Name {name} in bucket {bucket}",
                data.FileName,
                data.BucketName);

            return Errors.Files.FailUpload().ToErrorList();
        }
    }

    public async Task<Result<string, ErrorList>> GetPresignedUrlForDownload(FileMetadata fileMetadata,
        CancellationToken cancellationToken)
    {
        try
        {
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = fileMetadata.BucketName,
                Key = fileMetadata.Key,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddSeconds(Expiry),
                Protocol = Protocol.HTTP,
            };

            var url = await _client.GetPreSignedURLAsync(presignedRequest);

            if (url is null)
                return Errors.Files.NotFound().ToErrorList();

            return url;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to get file in minio");
            return Errors.Files.FailUpload().ToErrorList();
        }
    }

    public async Task<GetObjectMetadataResponse> GetObjectMetadata(string bucketName, string key,
        CancellationToken cancellationToken = default)
    {
        var metaDataRequest = new GetObjectMetadataRequest
        {
            BucketName = bucketName,
            Key = key
        };
        
            return await _client.GetObjectMetadataAsync(metaDataRequest, cancellationToken);
            
    }

    public async Task<Result<IReadOnlyCollection<string>, ErrorList>> DownloadFiles(IEnumerable<FileMetadata> filesData,
        CancellationToken cancellationToken = default)
    {
        var fileList = filesData.ToList();
        var semaphoreSlim = new SemaphoreSlim(MaxDegreeOfParallelism);

        await EnsureBucketExistsAsync(fileList.Select(f => f.BucketName), cancellationToken);

        var tasks = fileList.Select(async fileMetadata =>
            await GetPresignedUrlForDownload(fileMetadata, semaphoreSlim, cancellationToken));
        var results = await Task.WhenAll(tasks);

        var errors = results
            .Where(r => r.IsFailure)
            .Select(r => r.Error).ToList();

        if (errors.Count > 0)
            return new ErrorList(errors);

        _logger.LogInformation("Files uploaded to Minio");
        return results.Select(r => r.Value).ToList();
    }

    public async Task<UnitResult<ErrorList>> DeleteFile(
        string bucketName,
        string key,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            await _client.DeleteObjectAsync(request, cancellationToken);

            return Result.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can not delete file from minio");

            return Errors.Files.FailRemove().ToErrorList();
        }
    }

    public async Task<UnitResult<ErrorList>> DeleteFiles(IEnumerable<FileMetadata> files,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MaxDegreeOfParallelism);
        var filesList = files.ToList();

        try
        {
            var tasks = filesList.Select(async fileMetadata =>
                await DeleteFile(fileMetadata.Key, fileMetadata.BucketName, semaphoreSlim, cancellationToken));
            
            var results = await Task.WhenAll(tasks);
            
            var errors = results
                .Where(r => r.IsFailure)
                .Select(r => r.Error).ToList();

            if (errors.Count > 0)
                return new ErrorList(errors);

            return Result.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can not delete files from minio");

            return Errors.Files.FailRemove().ToErrorList();
        }
    }


    private async Task EnsureBucketExistsAsync(IEnumerable<string> bucketNames, CancellationToken cancellationToken)
    {
        HashSet<string> buckets = [..bucketNames];

        var response = await _client.ListBucketsAsync(cancellationToken);

        foreach (var bucketName in buckets)
        {
            var isExist = response.Buckets
                .Exists(b => b.BucketName.Equals(bucketName, StringComparison.OrdinalIgnoreCase));

            if (!isExist)
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucketName
                };

                await _client.PutBucketAsync(request, cancellationToken);
            }
        }
    }

    private async Task<UnitResult<Error>> UploadFile(
        UploadFileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            var request = new PutObjectRequest
            {
                BucketName = fileData.BucketName,
                Key = fileData.Key,
                InputStream = fileData.Stream,
                ContentType = fileData.ContentType
            };        

            await _client.PutObjectAsync(request, cancellationToken);

            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to upload file to Minio");
            return Errors.Files.FailUpload();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task<Result<string, Error>> GetPresignedUrlForDownload(
        FileMetadata fileMetadata,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            var presignedRequest = new GetPreSignedUrlRequest()
            {
                BucketName = fileMetadata.BucketName,
                Key = fileMetadata.Key,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddSeconds(Expiry),
                Protocol = Protocol.HTTP
            };

            var url = await _client.GetPreSignedURLAsync(presignedRequest);
            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to upload files to Minio");
            return Errors.Files.FailUpload();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
    
    private async Task<UnitResult<Error>> DeleteFile(
        string key,
        string bucketName,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            await _client.DeleteObjectAsync(request, cancellationToken);

            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can not delete file from minio");
            
            return Errors.Files.FailRemove();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
}