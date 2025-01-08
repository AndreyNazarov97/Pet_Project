using FileService.Communication.Contracts;
using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;

namespace FileService.Features;

public static class GetDownloadPresignedUrl
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/presigned-for-downloading", Handler);
        }
    }

    private static async Task<IResult> Handler(
        GetDownloadPresignedUrlRequest request,
        Guid key,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            Prefix = request.Prefix,
            Key = $"{request.Prefix}/{key}",
        };

        var result = await provider.GetPresignedUrlForDownload(fileMetadata, cancellationToken);

        return result.IsFailure 
            ? Results.BadRequest(result.Error.Errors) 
            : Results.Ok(result.Value);
    }
}