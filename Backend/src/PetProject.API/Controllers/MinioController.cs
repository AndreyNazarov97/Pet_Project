using Microsoft.AspNetCore.Mvc;
using PetProject.API.Extensions;
using PetProject.API.Providers;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;

namespace PetProject.API.Controllers;
[Controller]
[Route("minio")]
public class MinioController : ControllerBase
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
        try
        {
            await using var stream = file.OpenReadStream();
            var result = await _minioProvider.UploadFile(stream, BUCKET_NAME, file.FileName, cancellationToken);
       
            return result.ToResponse();
        }
        catch (Exception e)
        {
            var error = Errors.Minio.CouldNotUploadFile();
            return Result.Failure(error).ToResponse();
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteFile(string fileName, CancellationToken cancellationToken = default)
    {
        var result = await _minioProvider.DeleteFile(BUCKET_NAME, fileName, cancellationToken);
        
        return result.ToResponse();
    }

    [HttpGet]
    public async Task<ActionResult<string>> DownloadFile(string fileName)
    {
        var result = await _minioProvider.DownloadFile(BUCKET_NAME, fileName);
        
        return result.ToResponse();
    }
    
}