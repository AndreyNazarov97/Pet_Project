using Amazon.S3;
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

public static class CompleteMultipartUpload
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/complete-multipart", Handler);
        }
    }

    private static async Task<IResult> Handler(
        CompleteMultipartUploadRequest request,
        string key,
        IFileProvider fileProvider,
        IFilesRepository filesRepository,
        CancellationToken cancellationToken)
    {
        try
        {
            var data = new CompleteMultipartUploadData(request.BucketName, request.UploadId, key, request.Parts);
            
            var result = await fileProvider.CompleteMultipartUpload(
                data, cancellationToken);
            
            var fileId = Guid.NewGuid();

            var metaData = await fileProvider
                .GetObjectMetadata(request.BucketName, key, cancellationToken);

            var fileMetadata = new FileMetadata
            {
                Id = fileId,
                Key = key,
                Name = request.FileName,
                BucketName = request.BucketName,
                ContentType = metaData.Headers.ContentType,
                UploadDate = DateTime.UtcNow
            };

            await filesRepository.AddRangeAsync([fileMetadata], cancellationToken);

            BackgroundJob.Schedule<ConsistencyConfirmJob>(
                j => j.Execute(
                    fileMetadata.Id, fileMetadata.BucketName, fileMetadata.Key),
                TimeSpan.FromHours(24));

            var response = new CompleteMultipartUploadResponse
            {
                FileId = fileId,
                Location = result.Location
            };
        
            return Results.Ok(response);
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error completing multipart upload: {ex.Message}");
        }
    }
}