using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.Providers;

namespace PetProject.VolunteerManagement.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MaxDegreeOfParallelism = 10;
    private const int Expiry = 604800; // 7 days
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(
        IMinioClient minioClient,
        ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, ErrorList>> DownloadFile(FileMetaDataDto fileMetaData)
    {
        try
        {
            var getObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName)
                .WithExpiry(Expiry);

            _logger.LogInformation("File {FileName} downloading from Minio", fileMetaData.ObjectName);

            var downloadUrl = await _minioClient.PresignedGetObjectAsync(getObjectArgs);

            _logger.LogInformation(
                "File {FileName} downloaded from Minio with url {DownloadUrl}",
                fileMetaData.ObjectName,
                downloadUrl);
            return downloadUrl;
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "File {FileName} could not be downloaded from Minio",
                fileMetaData.ObjectName);
            return Errors.Minio.CouldNotDownloadFile().ToErrorList();
        }
    }

    public async Task<Result<IReadOnlyCollection<FilePath>, ErrorList>> UploadFiles(IEnumerable<FileDataDto> filesData,
        CancellationToken cancellationToken = default)
    {
        var fileList = filesData.ToList();
        var semaphoreSlim = new SemaphoreSlim(MaxDegreeOfParallelism);

        await EnsureBucketExistsAsync(fileList.Select(f => f.BucketName), cancellationToken);

        var tasks = fileList.Select(async fileData => await PutObject(fileData, semaphoreSlim, cancellationToken));
        var results = await Task.WhenAll(tasks);

        var errors = results
            .Where(r => r.IsFailure)
            .Select(r => r.Error).ToList();

        if (errors.Count > 0)
            return new ErrorList(errors);

        _logger.LogInformation("Files uploaded to Minio");
        return results.Select(r => r.Value).ToList();
    }

    [Obsolete("Obsolete")]
    public Result<IReadOnlyCollection<string>, ErrorList> GetFiles(string bucketName)
    {
        var listObjectsArgs = new ListObjectsArgs()
            .WithBucket(bucketName)
            .WithRecursive(false);

        var objects = _minioClient.ListObjectsAsync(listObjectsArgs);

        List<string> paths = [];

        using var subscription = objects.Subscribe(
            (item) => paths.Add(item.Key),
            ex => _logger.LogError(ex, "Error occured while getting objects"),
            () => _logger.LogInformation("Successfully uploaded files"));

        return paths;
    }

    public async Task<UnitResult<ErrorList>> DeleteFile(FileMetaDataDto fileMetaData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureBucketExistsAsync([fileMetaData.BucketName], cancellationToken);
            
            var statArgs = new StatObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName);
            
            var objectStat = await _minioClient.StatObjectAsync(statArgs, cancellationToken);
            if (objectStat == null)
            {
                return UnitResult.Success<ErrorList>();
            }
            
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName);


            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            _logger.LogInformation("File {FileName} deleted from Minio", fileMetaData.ObjectName);
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "File {FileName} could not be deleted from Minio", fileMetaData.ObjectName);
            return Errors.Minio.CouldNotDeleteFile().ToErrorList();
        }
    }

    private async Task EnsureBucketExistsAsync(IEnumerable<string> bucketNames, CancellationToken cancellationToken)
    {
        HashSet<string> buckets = [..bucketNames];

        foreach (var bucketName in buckets)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }

    private async Task<Result<FilePath, Error>> PutObject(
        FileDataDto fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var extension = Path.GetExtension(fileData.ObjectName);
        var filePath = FilePath.Create(Guid.NewGuid().ToString(), extension);
        if (filePath.IsFailure)
            return filePath.Error;

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(filePath.Value.Path);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            return filePath.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to upload files to Minio");
            return Errors.Minio.CouldNotUploadFile();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
}