using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;

namespace FileService.Features;

public static class DownloadPresignedUrl
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{id:guid}/presigned-for-downloading", Handler);
        }
    }

    private static async Task<IResult> Handler(
        Guid id,
        IFileProvider provider,
        IFilesRepository filesRepository,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = await filesRepository.GetById(id, cancellationToken);
        if (fileMetadata is null)
            return Results.NotFound("File not found");
        
        var result = await provider.GetPresignedUrlForDownload(fileMetadata, cancellationToken);

        return result.IsFailure 
            ? Results.BadRequest(result.Error.Errors) 
            : Results.Ok(result.Value);
    }
}