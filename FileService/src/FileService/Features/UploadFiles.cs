using FileService.Communication.Contracts.Requests;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;
using FileService.Processors;

namespace FileService.Features;

public static class UploadFiles
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/upload", Handler)
                .DisableAntiforgery();
        }
    }

    private static async Task<IResult> Handler(
        [AsParameters]UploadFileRequest request,
        IFormFileCollection files,
        IFileProvider provider,
        IFilesRepository filesRepository,
        CancellationToken cancellationToken)
    {
        await using var processor = new FormFileProcessor();
        processor.Process(files, request.BucketName);
        
        var uploadResult = await provider.UploadFiles(processor.FilesData, cancellationToken);
        
        if (uploadResult.IsFailure)
            return Results.BadRequest(uploadResult.Error.Errors);
        
        await filesRepository.AddRangeAsync(processor.FilesMetaData, cancellationToken);
        
        return Results.Ok(processor.Responses);
    }
}