using Microsoft.AspNetCore.Mvc;
using PetProject.API.Extensions;
using PetProject.API.Response;
using PetProject.Application.Abstractions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.API.Controllers;

public class MinioController : ApplicationController
{
    public const string BucketName = "pet-project";
    private readonly IFileProvider _fileProvider;

    public MinioController(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    [HttpPost]
    public async Task<ActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken = default)
    {
        await using var stream = file.OpenReadStream();
        var fileData = new FileDataDto(stream, file.FileName, BucketName);
        
        var result = await _fileProvider.UploadFile(fileData, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteFile(string fileName, CancellationToken cancellationToken = default)
    {
        var fileMetaData = new FileMetaDataDto(fileName, BucketName);
        var result = await _fileProvider.DeleteFile(fileMetaData, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<string>> DownloadFile(string fileName)
    {
        var fileMetaData = new FileMetaDataDto(fileName, BucketName);
        
        var result = await _fileProvider.DownloadFile(fileMetaData);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result);
    }
}