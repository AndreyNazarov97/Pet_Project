namespace PetProject.Domain.Shared;

public sealed class Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType  Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new InvalidOperationException();

        Code = code;
        Message = message;
        Type = type;
    }
    
    public static Error Validation(string errorCode, string errorMessage) => 
        new(errorCode, errorMessage, ErrorType.Validation);
    public static Error NotFound(string errorCode, string errorMessage) =>
        new(errorCode, errorMessage, ErrorType.NotFound);
    public static Error Failure(string errorCode, string errorMessage) => 
        new(errorCode, errorMessage, ErrorType.Failure);
    public static Error Conflict(string errorCode, string errorMessage) => 
        new(errorCode, errorMessage, ErrorType.Conflict);

    public override string ToString()
    {
        return $"ErrorCode: {Code}. ErrorMessage:{Message}";
    }
}

public enum ErrorType
{
    Validation,
    NotFound,
    Failure,
    Conflict
}