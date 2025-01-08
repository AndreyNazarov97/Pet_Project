using FileService.Communication.Contracts.Requests;
using FileService.Communication.Contracts.Responses;
using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Providers.Data;
using FileService.Infrastructure.Repositories;
using FileService.Jobs;
using Hangfire;

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
        IFilesRepository filesRepository,
        CancellationToken cancellationToken)
    {
        var fileExtension = Path.GetExtension(request.FileName);

        var key = $"{Guid.NewGuid()}{fileExtension}";
        
        var data = new GetPresignedUrlForUploadData(request.BucketName, request.FileName, key, request.ContentType);

        var presignedUrlResult = await provider.GetPresignedUrlForUpload(data, cancellationToken);
        
        if (presignedUrlResult.IsFailure)
            return Results.BadRequest(presignedUrlResult.Error.Errors);

        var fileId = Guid.NewGuid();
        
        var fileMetadata = new FileMetadata
        {
            Id = fileId,
            Key = key,
            Name = request.FileName,
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            UploadDate = DateTime.UtcNow
        };

        await filesRepository.AddRangeAsync([fileMetadata], cancellationToken);
        
        BackgroundJob.Schedule<ConsistencyConfirmJob>(
            j => j.Execute(
                fileMetadata.Id, fileMetadata.BucketName, fileMetadata.Key),
            TimeSpan.FromHours(24));

        var response = new UploadPresignedUrlResponse
        {
            FileId = fileId,
            Url = presignedUrlResult.Value
        };

        return Results.Ok(response);
    }
}