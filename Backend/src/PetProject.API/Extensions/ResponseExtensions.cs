using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;
using PetProject.Domain.Shared;

namespace PetProject.API.Extensions;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this Result result)
    {
        if (result.IsSuccess)
            return new OkResult();
        
        var statusCode = result.Error!.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var envelope = Envelope.Error(result.Error);
        
        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }
    
    public static ActionResult<T> ToResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(Envelope.Ok(result.Value));
        }
            
        
        var statusCode = result.Error!.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var envelope = Envelope.Error(result.Error);
        
        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }
    
}