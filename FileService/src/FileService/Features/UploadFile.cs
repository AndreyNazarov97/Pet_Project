using System.Net.Http.Headers;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;

namespace FileService.Features;


public static class UploadFile
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/upload", Handler)
                .DisableAntiforgery();
        }
    }

    private static async Task<IResult> Handler(
        string presignedUrl,
        IFormFile file,
        IFileProvider provider,
        CancellationToken cancellationToken)
    {
        try
        {
            // Загрузка файла по полученной URL
            await using var stream = file.OpenReadStream();
            using var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            using var httpClient = new HttpClient();
            var uploadResponse = await httpClient.PutAsync(presignedUrl, content, cancellationToken);

            if (!uploadResponse.IsSuccessStatusCode)
            {
                return Results.Problem($"Ошибка при загрузке файла: {uploadResponse.ReasonPhrase}");
            }

            return Results.Ok(new { Message = "Файл успешно загружен." });
        }
        catch (Exception ex)
        {
            return Results.Problem($"Ошибка при обработке запроса: {ex.Message}");
        }
    }
}