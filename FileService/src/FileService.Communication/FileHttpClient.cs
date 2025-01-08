using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using FileService.Communication.Contracts;
using FileService.Communication.Contracts.Requests;
using FileService.Communication.Contracts.Responses;

namespace FileService.Communication;

public class FileHttpClient(HttpClient httpClient)
{
    public async Task<Result<string>> GetFileUrlAsync(
        GetDownloadPresignedUrlRequest request, Guid key, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            $"files/{key}/presigned-for-downloading", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
            return Result.Failure<string>(response.ReasonPhrase!);

        return await response.Content
            .ReadFromJsonAsync<string>(cancellationToken: cancellationToken)!;
    }


    public async Task<Result<IReadOnlyList<string>>> GetFilesUrlsByIdsAsync(
        GetFilesByIdsRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "files/files-by-ids", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
            return Result.Failure<IReadOnlyList<string>>(response.ReasonPhrase!);

        var fileResponse = await response.Content
            .ReadFromJsonAsync<IEnumerable<string>>(cancellationToken: cancellationToken)!;

        return fileResponse?.ToList() ?? [];
    }

    public async Task<Result<StartMultipartUploadResponse>> StartMultipartUploadAsync(
        StartMultipartUploadRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "files/multipart-upload", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
            return Result.Failure<StartMultipartUploadResponse>(response.ReasonPhrase!);

        return await response.Content
            .ReadFromJsonAsync<StartMultipartUploadResponse>(cancellationToken: cancellationToken)!;
    }

    public async Task<Result<string>> UploadPresignedPartUrlAsync(
        UploadPresignedPartUrlRequest request, Guid key, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            $"files/{key}/presigned-part", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
            return Result.Failure<string>(response.ReasonPhrase!);

        return await response.Content
            .ReadFromJsonAsync<string>(cancellationToken: cancellationToken)!;
    }

    public async Task<Result<CompleteMultipartUploadResponse>> CompleteMultipartUploadAsync(
        CompleteMultipartUploadRequest request, Guid key, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            $"files/{key}/complete-multipart", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
            return Result.Failure<CompleteMultipartUploadResponse>(response.ReasonPhrase!);

        return await response.Content
            .ReadFromJsonAsync<CompleteMultipartUploadResponse>(cancellationToken: cancellationToken)!;
    }

    public async Task<Result<UploadPresignedUrlResponse>> UploadPresignedUrlAsync(
        UploadPresignedUrlRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "files/presigned-upload", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
            return Result.Failure<UploadPresignedUrlResponse>(response.ReasonPhrase!);

        return await response.Content
            .ReadFromJsonAsync<UploadPresignedUrlResponse>(cancellationToken: cancellationToken)!;
    }

    public async Task<Result<string>> DeletePresignedUrlAsync(
        DeleteFilesRequest request, Guid key, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            $"files/{key}/presigned-for-deletion", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
            return Result.Failure<string>(response.ReasonPhrase!);

        return await response.Content
            .ReadFromJsonAsync<string>(cancellationToken: cancellationToken)!;
    }

    public async Task<Result> UploadFileToPresignedUrlAsync(
        UploadFileRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            using var stream = request.File.OpenReadStream();
            using var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue(request.File.ContentType);

            using var response = await httpClient.PutAsync(request.PresignedUrl, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure($"Ошибка при загрузке файла: {response.ReasonPhrase}");
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Исключение при загрузке файла: {ex.Message}");
        }
    }
}