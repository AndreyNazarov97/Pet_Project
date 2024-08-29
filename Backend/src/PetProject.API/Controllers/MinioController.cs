using Microsoft.AspNetCore.Mvc;
using PetProject.API.Extensions;
using PetProject.API.Providers;
using PetProject.API.Response;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;

namespace PetProject.API.Controllers;

public class MinioController : ApplicationController
{
    public const string BUCKET_NAME = "pet-project";
    private readonly IMinioProvider _minioProvider;

    public MinioController(IMinioProvider minioProvider)
    {
        _minioProvider = minioProvider;
    }

    [HttpPost]
    public async Task<ActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken = default)
    {
        await using var stream = file.OpenReadStream();
        var result = await _minioProvider.UploadFile(stream, BUCKET_NAME, file.FileName, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteFile(string fileName, CancellationToken cancellationToken = default)
    {
        var result = await _minioProvider.DeleteFile(BUCKET_NAME, fileName, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<string>> DownloadFile(string fileName)
    {
        var result = await _minioProvider.DownloadFile(BUCKET_NAME, fileName);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result);
    }
}