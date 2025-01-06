using System.Net;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using FileService.Communication.Contracts;

namespace FileService.Communication;

public class FileHttpClient(HttpClient httpClient)
{
    public async Task<Result<IReadOnlyList<string>>> GetFilesUrlsByIdsAsync(
        GetFilesByIdsRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "files/files-by-ids", request, cancellationToken);
        
        if(response.StatusCode != HttpStatusCode.OK)
            return Result.Failure<IReadOnlyList<string>>(response.ReasonPhrase!);
        
        var fileResponse = await response.Content
            .ReadFromJsonAsync<IEnumerable<string>>(cancellationToken: cancellationToken)!;

        return fileResponse?.ToList() ?? [];
    }
}