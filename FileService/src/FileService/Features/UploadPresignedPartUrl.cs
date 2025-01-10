using FileService.Communication.Contracts.Requests;
using FileService.Communication.Contracts.Responses;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Providers.Data;

namespace FileService.Features;

public static class UploadPresignedPartUrl
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/presigned-part", Handler);
        }
    }

    private static async Task<IResult> Handler(
        UploadPresignedPartUrlRequest request,
        string key,
        IFileProvider fileProvider,
        CancellationToken cancellationToken)
    {
        var data = new GetPresignedUrlForUploadPartData(request.BucketName, key, request.UploadId, request.PartNumber);
        
        var presignedUrlResult = await fileProvider.GetPresignedUrlPart(data, cancellationToken);
        
        if(presignedUrlResult.IsFailure)
            return Results.BadRequest(presignedUrlResult.Error.Errors);

        var response = new UploadPresignedPartUrlResponse
        {
            Key = key,
            Url = presignedUrlResult.Value
        };

        return Results.Ok(response);
    }
}