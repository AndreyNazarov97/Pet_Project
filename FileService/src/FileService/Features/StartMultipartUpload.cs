using FileService.Communication.Contracts;
using FileService.Communication.Contracts.Requests;
using FileService.Communication.Contracts.Responses;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Providers.Data;

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
        var fileExtension = Path.GetExtension(request.FileName);

        var key = $"{Guid.NewGuid()}{fileExtension}";

        var data = new StartMultipartUploadData(request.BucketName, request.FileName, key, request.ContentType);

        var result = await fileProvider.StartMultipartUpload(data, cancellationToken);

        var response = new StartMultipartUploadResponse
        {
            Key = result.Key,
            UploadId = result.UploadId
        };

        return Results.Ok(response);
    }
}