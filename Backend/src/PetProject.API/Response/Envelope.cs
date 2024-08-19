using PetProject.Domain.Shared;

namespace PetProject.API.Response;

public record Envelope
{
    public object? Result { get; }
    public string? ErrorMessage { get; }
    public string? ErrorCode { get; }
    public DateTimeOffset CreatedAt { get; }

    private Envelope(object? result, Error? error)
    {
        Result = result;
        ErrorMessage = error?.Message;
        ErrorCode = error?.Code;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Envelope Ok(object? result) => new(result, null);

    public static Envelope Error(Error error) => new(null, error);
}