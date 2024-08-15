﻿namespace PetProject.Domain.Shared;

public sealed class Error
{
    public string ErrorCode { get; } 
    public string ErrorMessage { get; }

    public Error(string errorCode, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorCode))
            throw new InvalidOperationException();
        
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public override string ToString()
    {
        return $"ErrorCode: {ErrorCode}.\nErrorMessage:{ErrorMessage}";
    }
}