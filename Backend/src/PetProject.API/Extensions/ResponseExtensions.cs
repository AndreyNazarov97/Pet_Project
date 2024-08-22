using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;
using PetProject.Domain.Shared;

namespace PetProject.API.Extensions;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this Result result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(Envelope.Ok("Ok"));

        var statusCode = result.Error!.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var responseError = new ResponseError(result.Error.Code, result.Error.Message, null);

        var envelope = Envelope.Error([responseError]);

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

        var responseError = new ResponseError(result.Error.Code, result.Error.Message, null);

        var envelope = Envelope.Error([responseError]);

        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }

    public static ActionResult ToValidationErrorResponse(this ValidationResult validationResult)
    {
        if (validationResult.IsValid)
            throw new InvalidOperationException("Result cannot be succed");

        var validationErrors = validationResult.Errors;

        var responseErrors =
            from validationError in validationErrors
            let error = Error.Deserialize(validationError.ErrorMessage)
            select new ResponseError(error.Code, error.Message, validationError.PropertyName);

        var envelope = Envelope.Error(responseErrors);

        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}