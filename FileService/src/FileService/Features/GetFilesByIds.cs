using FileService.Communication.Contracts.Requests;
using FileService.Communication.Contracts.Responses;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;
using FileInfo = FileService.Communication.Contracts.Responses.FileInfo;

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
        
        var fileInfos = files.Zip(result.Value, (file, url) => 
                new FileInfo(file.Id, url, file.Key, file.UploadDate))
            .ToList();

        var response = new GetFilesByIdsResponse(fileInfos);
        
        return Results.Ok(response);
    }
}