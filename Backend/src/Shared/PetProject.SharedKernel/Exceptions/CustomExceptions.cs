using PetProject.SharedKernel.Shared;

namespace PetProject.SharedKernel.Exceptions;

public abstract class CustomException : Exception
{
    public string Code { get; }
    
    public ErrorType Type { get; }

    public CustomException(Error error) 
        : base(error.Message)
    {
        Code = error.Code;
        Type = error.Type;
    }
}