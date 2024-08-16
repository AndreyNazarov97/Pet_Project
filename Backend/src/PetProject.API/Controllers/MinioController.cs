using Microsoft.AspNetCore.Mvc;
using PetProject.API.Providers;

namespace PetProject.API.Controllers;
[Controller]
[Route("minio")]
public class MinioController : ControllerBase
{
    public const string BUCKET_NAME = "pet-project";
    private readonly MinioProvider _minioProvider;
    public MinioController(MinioProvider minioProvider)
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
    
}