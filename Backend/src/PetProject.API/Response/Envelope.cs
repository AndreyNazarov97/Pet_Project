using PetProject.Domain.Shared;

namespace PetProject.API.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    public object? Result { get; }
    public List<ResponseError> Errors { get; }
    public DateTimeOffset CreatedAt { get; }

    private Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors.ToList();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Envelope Ok(object? result) => new(result, []);

    public static Envelope Error(IEnumerable<ResponseError> errors ) => new(null, errors);
}