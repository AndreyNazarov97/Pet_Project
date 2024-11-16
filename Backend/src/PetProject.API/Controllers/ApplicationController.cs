using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;

namespace PetProject.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public abstract class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);
        
        return new OkObjectResult(envelope);
    }
}