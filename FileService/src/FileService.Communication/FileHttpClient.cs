using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using FileService.Communication.Contracts.Requests;
using FileService.Communication.Contracts.Responses;
using Microsoft.AspNetCore.Http;

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

    public async Task<Result<UploadFileResponse[]>> UploadFilesAsync(
        UploadFileRequest request, IFormFileCollection files, CancellationToken cancellationToken = default)
    {
        // Создаём multipart/form-data контент
        using var content = new MultipartFormDataContent();

        // Добавляем метаинформацию из запроса
        var bucketNameContent = new StringContent(request.BucketName);
        content.Add(bucketNameContent, nameof(request.BucketName));

        // Добавляем файлы в form-data
        foreach (var file in files)
        {
            var fileStream = file.OpenReadStream();
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, nameof(files), file.FileName);
        }
        
        using var response = await httpClient.PostAsync("files/upload", content, cancellationToken);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return Result.Failure<UploadFileResponse[]>(response.ReasonPhrase!);
        
        return await response.Content
            .ReadFromJsonAsync<UploadFileResponse[]>(cancellationToken: cancellationToken)!;
    }
}