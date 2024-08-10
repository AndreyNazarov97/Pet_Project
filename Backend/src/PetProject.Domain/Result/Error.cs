namespace PetProject.Domain.Result;

public sealed class Error
{
    public string Code { get; }
    public string Description { get; }
    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }
    
    public static readonly Error None = new(string.Empty, string.Empty);
}