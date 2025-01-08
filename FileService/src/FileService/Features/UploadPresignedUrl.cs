using FileService.Communication.Contracts;
using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;

namespace FileService.Features;

public static class UploadPresignedUrl
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned-upload", Handler);
        }
    }

    private static async Task<IResult> Handler(
        UploadPresignedUrlRequest request,
        IFileProvider provider,
        CancellationToken cancellationToken)
    {
        var key = Guid.NewGuid();
        
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Name = request.FileName,
            Prefix = request.Prefix,
            Key = $"{request.Prefix}/{key}",
        };
        
        var result = await provider.GetPresignedUrlForUpload(fileMetadata, cancellationToken); 
        return Results.Ok(new UploadPresignedUrlResponse
        {
            Key = key.ToString(),
            Url = result.Value
        });
    }
}