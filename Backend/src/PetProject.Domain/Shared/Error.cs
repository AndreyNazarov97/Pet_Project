namespace PetProject.Domain.Shared;

public sealed class Error
{
    private const string SEPARATOR = "||";
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error None() => new(string.Empty, string.Empty, ErrorType.None);

    public static Error Validation(string errorCode, string errorMessage) =>
        new(errorCode, errorMessage, ErrorType.Validation);

    public static Error NotFound(string errorCode, string errorMessage) =>
        new(errorCode, errorMessage, ErrorType.NotFound);

    public static Error Failure(string errorCode, string errorMessage) =>
        new(errorCode, errorMessage, ErrorType.Failure);

    public static Error Conflict(string errorCode, string errorMessage) =>
        new(errorCode, errorMessage, ErrorType.Conflict);

    public static Error Deserialize(string serializedError)
    {
        var parts = serializedError.Split(SEPARATOR);
        if (parts.Length != 3)
        {
            throw new ArgumentException("Invalid serialized format");
        }

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
        {
            throw new ArgumentException("Invalid error type");
        }

        return new Error(parts[0], parts[1], type);
    }

    public string Serialize()
    {
        return string.Join(SEPARATOR, Code, Message, Type);
    }


    public override string ToString()
    {
        return $"ErrorCode: {Code}. ErrorMessage:{Message}";
    }
}

public enum ErrorType
{
    None,
    Validation,
    NotFound,
    Failure,
    Conflict
}