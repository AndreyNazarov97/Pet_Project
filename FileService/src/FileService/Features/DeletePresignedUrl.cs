﻿using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;

namespace FileService.Features;

public static class DeletePresignedUrl
{
    private record DeletePresignedUrlRequest(
        string BucketName,
        string FileName, 
        string Extension,
        string ContentType);
    
    public sealed class Endpoint: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/presigned-for-deletion", Handler);
        }
    }

    private static async Task<IResult> Handler( 
        DeletePresignedUrlRequest request,
        Guid key,
        IFileRepository fileRepository,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            Name = request.FileName,
            Key = $"{key}.{request.Extension}",
            Extension = request.Extension,
            ContentType = request.ContentType
        };
        
        var result = await provider.GetPresignedUrlForDelete(fileMetadata, cancellationToken); 
        
        return Results.Ok(new
        {
            key,
            url = result.Value
        });
    }
}