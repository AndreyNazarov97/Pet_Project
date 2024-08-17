using Microsoft.AspNetCore.Mvc;
using PetProject.API.Providers;
using PetProject.Application.Abstractions;

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
    public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken = default)
    {
        using var stream = file.OpenReadStream();
        await _minioProvider.UploadFile(stream, BUCKET_NAME, file.FileName, cancellationToken);
        return Ok(file.FileName);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFile(string fileName, CancellationToken cancellationToken = default)
    {
        await _minioProvider.DeleteFile(BUCKET_NAME, fileName, cancellationToken);
        return Ok(fileName);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        var url = await _minioProvider.DownloadFile(BUCKET_NAME, fileName);
        return Ok(url);
    }
    
}