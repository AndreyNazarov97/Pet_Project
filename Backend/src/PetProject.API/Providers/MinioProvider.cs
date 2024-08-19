using Minio;
using Minio.DataModel.Args;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;

namespace PetProject.API.Providers;

public class MinioProvider : IMinioProvider
{
    private const int EXPIRY = 604800; // 7 days
    private readonly IMinioClient _minioClient;

    public MinioProvider(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<string> DownloadFile(string bucketName, string fileName)
    {
        var getObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithExpiry(EXPIRY);

        var downloadUrl = await _minioClient.PresignedGetObjectAsync(getObjectArgs);
        return downloadUrl;
    }

    public async Task<Result> UploadFile(Stream stream, string bucketName, string fileName,
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

            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(new Error("Could not upload file", e.Message));
        }
    }

    public async Task DeleteFile(string bucketName, string fileName, CancellationToken cancellationToken = default)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName);
        await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
    }
}