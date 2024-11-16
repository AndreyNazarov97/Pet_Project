using System.Net;
using PetProject.API.Response;
using PetProject.Domain.Shared;
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
        var error = Error.Failure("server.internal", exception.Message);
        var envelope = Envelope.Error(error.ToErrorList());

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(envelope);
    }
}
public static class ExceptionHandlingMiddlewareExtensions
{
    public static void UseExceptionHandling(
        this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}