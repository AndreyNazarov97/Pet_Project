using Minio;
using Minio.DataModel.Args;

namespace PetProject.API.Providers;

public class MinioProvider
{
    private readonly IMinioClient _minioClient;

    public MinioProvider(IConfiguration configuration)
    {
        var endpoint = configuration.GetValue<string>("Minio:Endpoint");
        var accessKey = configuration.GetValue<string>("Minio:AccessKey");
        var secretKey = configuration.GetValue<string>("Minio:SecretKey");

        _minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .Build();
    }

    public async Task<string> DownloadFile(string bucketName, string fileName)
    {
        var getObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithExpiry(60 * 60 * 24 * 7);

        var downloadUrl = await _minioClient.PresignedGetObjectAsync(getObjectArgs);
        return downloadUrl;
    }

    public async Task UploadFile(Stream stream, string bucketName, string fileName,
        CancellationToken cancellationToken = default)
    {
        if (!await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), cancellationToken))
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName), cancellationToken);
        }

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType("application/octet-stream");

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
    }

    public async Task DeleteFile(string bucketName, string fileName, CancellationToken cancellationToken = default)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName);
        await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
    }
}