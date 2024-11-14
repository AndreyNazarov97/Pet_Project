using CSharpFunctionalExtensions;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using PetProject.Application.Abstractions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;
using ILogger = Serilog.ILogger;


namespace PetProject.Infrastructure.Postgres.Providers;

public class MinioProvider : IFileProvider
{
    private const int MaxDegreeOfParallelism = 10;
    private const int Expiry = 604800; // 7 days
    private readonly IMinioClient _minioClient;
    private readonly ILogger _logger;

    public MinioProvider(
        IMinioClient minioClient,
        ILogger logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> DownloadFile(FileMetaDataDto fileMetaData)
    {
        try
        {
            var getObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName)
                .WithExpiry(Expiry);

            _logger.Information("File {FileName} downloading from Minio", fileMetaData.ObjectName);

            var downloadUrl = await _minioClient.PresignedGetObjectAsync(getObjectArgs);

            _logger.Information(
                "File {FileName} downloaded from Minio with url {DownloadUrl}",
                fileMetaData.ObjectName,
                downloadUrl);
            return downloadUrl;
        }
        catch (Exception e)
        {
            _logger.Error(e,
                "File {FileName} could not be downloaded from Minio",
                fileMetaData.ObjectName);
            return Errors.Minio.CouldNotDownloadFile();
        }
    }

    public async Task<Result<IReadOnlyCollection<FilePath>,Error>> UploadFiles(IEnumerable<FileDataDto> filesData,
        CancellationToken cancellationToken = default)
    {
        var fileList = filesData.ToList();
        var semaphoreSlim = new SemaphoreSlim(MaxDegreeOfParallelism);

        await EnsureBucketExistsAsync(fileList.Select(f => f.BucketName), cancellationToken);
    
        var tasks = fileList.Select(async fileData => await PutObject(fileData, semaphoreSlim, cancellationToken));
        var results = await Task.WhenAll(tasks);
        
        if(results.Any(r => r.IsFailure))
            return results.First(r => r.IsFailure).Error;
        
        _logger.Information("Files uploaded to Minio");
        return results.Select(r => r.Value).ToList();
    }

    public async Task<UnitResult<Error>> DeleteFile(FileMetaDataDto fileMetaData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName);

            _logger.Information("File {FileName} deleting from Minio", fileMetaData.ObjectName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            _logger.Information("File {FileName} deleted from Minio", fileMetaData.ObjectName);
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.Error(e, "File {FileName} could not be deleted from Minio", fileMetaData.ObjectName);
            return Errors.Minio.CouldNotDeleteFile();
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

    private async Task<Result<FilePath,Error>> PutObject(
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
            _logger.Error(ex, "Fail to upload files to Minio");
            return Errors.Minio.CouldNotUploadFile();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
}