using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;

namespace FileService.Features;

public static class DownloadPresignedUrl
{
    private record DownloadPresignedUrlRequest(
        string BucketName,
        string FileName, 
        string ContentType,
        string Prefix,
        string Extension);
    
    public sealed class Endpoint: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/presigned-for-downloading", Handler);
        }
    }

    private static async Task<IResult> Handler( 
        DownloadPresignedUrlRequest request,
        Guid key,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Name = request.FileName,
            Prefix = request.Prefix,
            Key = $"{key}.{request.Extension}",
            Extension = request.Extension
        };
        
        var result = await provider.GetPresignedUrlForDownload(fileMetadata, cancellationToken); 
        return Results.Ok(new
        {
            key,
            url = result.Value
        });
    }
}