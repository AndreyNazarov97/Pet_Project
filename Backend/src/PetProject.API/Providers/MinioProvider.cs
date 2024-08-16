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
    
    public async Task UploadFile(Stream stream, string bucketName, string fileName, CancellationToken cancellationToken = default)
    {
        if (!await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), cancellationToken))
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName), cancellationToken);
        }
        
        var args = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)  
            .WithContentType("application/octet-stream");
        
        await _minioClient.PutObjectAsync(args, cancellationToken);
    }
}