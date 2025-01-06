using FileService.Communication.Contracts;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;

namespace FileService.Features;

public static class GetFilesByIds
{
    public sealed class Endpoint: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/files-by-ids", Handler);
        }
    }

    private static async Task<IResult> Handler( 
        GetFilesByIdsRequest request,
        IFileProvider fileProvider,
        IFilesRepository filesRepository,
        CancellationToken cancellationToken = default)
    {
        var files = await filesRepository.Get(request.FileIds, cancellationToken);

        var result = await fileProvider.DownloadFiles(files, cancellationToken);
        if (result.IsFailure)
            return Results.Conflict(error: result.Error.Errors);
        
        files = files.Zip(result.Value,(file,url) => 
        { 
            file.DownloadUrl = url;
            return file;
        }).ToList();
        
        return Results.Ok(files);
    }
}