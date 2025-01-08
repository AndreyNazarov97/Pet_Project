﻿using Amazon.S3;
using FileService.Communication.Contracts;
using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
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
            var fileId = Guid.NewGuid();

            var fileMetadata = new FileMetadata
            {
                BucketName = request.BucketName,
                ContentType = request.ContentType,
                Name = request.FileName,
                Prefix = request.Prefix,
                Key = $"{request.Prefix}/{key}",
                UploadId = request.UploadId,
                ETags = request.Parts.Select(e => new ETagInfo { PartNumber = e.PartNumber, ETag = e.ETag })
            };

            var response = await fileProvider.CompleteMultipartUpload(
                fileMetadata, cancellationToken);

            var downloadUrl = await fileProvider.GetPresignedUrlForDownload(
                fileMetadata, cancellationToken);
            if(downloadUrl.IsFailure)
                return Results.BadRequest(downloadUrl.Error.Errors);

            fileMetadata.Id = fileId;
            fileMetadata.DownloadUrl = downloadUrl.Value;
            fileMetadata.Size = response.ContentLength;

            await filesRepository.AddRangeAsync([fileMetadata], cancellationToken);

            BackgroundJob.Schedule<ConsistencyConfirmJob>(
                j => j.Execute(
                    fileMetadata.Id, fileMetadata.BucketName, fileMetadata.Key),
                TimeSpan.FromHours(24));

            return Results.Ok(new CompleteMultipartUploadResponse
            {
                Location = response.Location
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error completing multipart upload: {ex.Message}");
        }
    }
}