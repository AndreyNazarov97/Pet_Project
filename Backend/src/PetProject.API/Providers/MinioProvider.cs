using CSharpFunctionalExtensions;
using Minio;
using Minio.DataModel.Args;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;
using ILogger = Serilog.ILogger;


namespace PetProject.API.Providers;

public class MinioProvider : IMinioProvider
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

    public async Task<Result<string, Error>> DownloadFile(string bucketName, string fileName)
    {
        try
        {
            var getObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithExpiry(EXPIRY);
            
            _logger.Information("File {FileName} downloading from Minio", fileName);
            
            var downloadUrl = await _minioClient.PresignedGetObjectAsync(getObjectArgs);
            
            _logger.Information("File {FileName} downloaded from Minio with url {DownloadUrl}", fileName, downloadUrl);
            return downloadUrl;
        }
        catch (Exception e)
        {
            _logger.Error(e, "File {FileName} could not be downloaded from Minio", fileName);
            return Errors.Minio.CouldNotDownloadFile();
        }
        
    }

    public async Task<UnitResult<Error>> UploadFile(Stream stream, string bucketName, string fileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
            var isBucketExist = !await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

            if (isBucketExist)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("application/octet-stream");

            _logger.Information("File {FileName} uploading to Minio", fileName);
            
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            
            _logger.Information("File {FileName} uploaded to Minio", fileName);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.Error(e, "File {FileName} could not be uploaded to Minio", fileName);
            return Errors.Minio.CouldNotUploadFile();
        }
    }

    public async Task<UnitResult<Error>> DeleteFile(string bucketName, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);
            
            _logger.Information("File {FileName} deleting from Minio", fileName);
            
            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            
            _logger.Information("File {FileName} deleted from Minio", fileName);
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.Error(e, "File {FileName} could not be deleted from Minio", fileName);
            return Errors.Minio.CouldNotDeleteFile();
        }
        
    }
}