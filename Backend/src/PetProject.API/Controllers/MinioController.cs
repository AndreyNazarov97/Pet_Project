using Microsoft.AspNetCore.Mvc;
using PetProject.API.Extensions;
using PetProject.API.Response;
using PetProject.Application.Abstractions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Infrastructure.Postgres.Processors;

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
    public async Task<ActionResult> UploadFiles(
        IFormFileCollection files,
        CancellationToken cancellationToken = default)
    {
        await using var fileProcessor = new FormFileProcessor();
        var filesDto = fileProcessor.Process(files);
        var filesData = filesDto.Select(f => new FileDataDto(f.Content, f.FileName, BucketName)).ToList();
        
        var result = await _fileProvider.UploadFiles(filesData, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
    
        return Ok(result.Value);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteFile(
        string fileName, 
        CancellationToken cancellationToken = default)
    {
        var fileMetaData = new FileMetaDataDto(fileName, BucketName);
        var result = await _fileProvider.DeleteFile(fileMetaData, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpGet("files")]
    public async Task<ActionResult<string[]>> GetList()
    {
        var result = await _fileProvider.GetFiles(BucketName);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<ActionResult<string>> DownloadFile(string fileName)
    {
        var fileMetaData = new FileMetaDataDto(fileName, BucketName);
        
        var result = await _fileProvider.DownloadFile(fileMetaData);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}