using System.Net;
using PetProject.API.Response;
using ILogger = Serilog.ILogger;

namespace PetProject.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.Error(exception, exception.Message);
        var responseError = new ResponseError("server.internal", exception.Message, null);
        var envelope = Envelope.Error([responseError]);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(envelope);
    }
}