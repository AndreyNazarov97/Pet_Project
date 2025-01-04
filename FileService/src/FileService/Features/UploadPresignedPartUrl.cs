using Amazon.S3;
using Amazon.S3.Model;
using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;

namespace FileService.Features;

public static class UploadPresignedPartUrl
{
    private record UploadPresignedPartUrlRequest(
        string UploadId,
        int PartNumber,
        string BucketName,
        string ContentType,
        string Prefix,
        string FileName);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/presigned-part", Handler);
        }
    }

    private static async Task<IResult> Handler(
        UploadPresignedPartUrlRequest request,
        Guid key,
        IFileProvider fileProvider,
        CancellationToken cancellationToken)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Name = request.FileName,
            Prefix = request.Prefix,
            Key = $"{request.Prefix}/{key}",
            UploadId = request.UploadId,
            PartNumber = request.PartNumber
        };
        
        
        var response = await fileProvider
            .GetPresignedUrlPart(fileMetadata, cancellationToken);
        
        return Results.Ok(new
        { 
            response
        });
    }
}