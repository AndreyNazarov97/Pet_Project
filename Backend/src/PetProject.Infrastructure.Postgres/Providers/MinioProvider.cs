using CSharpFunctionalExtensions;
using Minio;
using Minio.DataModel.Args;
using PetProject.Application.Abstractions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using ILogger = Serilog.ILogger;


namespace PetProject.Infrastructure.Postgres.Providers;

public class MinioProvider : IFileProvider
{
    private const int EXPIRY = 604800; // 7 days
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
                .WithExpiry(EXPIRY);
            
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

    public async Task<UnitResult<Error>> UploadFile(FileDataDto fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(fileData.BucketName);
            var isBucketExist = !await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

            if (isBucketExist)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(fileData.BucketName);
                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileData.ObjectName)
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithContentType("application/octet-stream");

            _logger.Information("File {FileName} uploading to Minio", fileData.ObjectName);
            
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            
            _logger.Information("File {FileName} uploaded to Minio", fileData.ObjectName);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.Error(e, "File {FileName} could not be uploaded to Minio", fileData.ObjectName);
            return Errors.Minio.CouldNotUploadFile();
        }
    }

    public async Task<UnitResult<Error>> DeleteFile(FileMetaDataDto fileMetaData, CancellationToken cancellationToken = default)
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
}