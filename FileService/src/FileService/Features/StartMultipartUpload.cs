using FileService.Communication.Contracts;
using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;

namespace FileService.Features;

public static class StartMultipartUpload
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/multipart-upload", Handler);
        }
    }

    private static async Task<IResult> Handler(
        StartMultipartUploadRequest request,
        IFileProvider fileProvider,
        CancellationToken cancellationToken)
    {
        var key = Guid.NewGuid();

        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Name = request.FileName,
            Prefix = request.Prefix,
            Key = $"{request.Prefix}/{key}"
        };

        var response = await fileProvider.StartMultipartUpload(fileMetadata, cancellationToken);

        return Results.Ok(new StartMultipartUploadResponse
        {
            Key = key.ToString(),
            UploadId = response.UploadId,
            BucketName = response.BucketName,
            Prefix = request.Prefix
        });
    }
}