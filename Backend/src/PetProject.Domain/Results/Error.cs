using System.Runtime.InteropServices.JavaScript;

namespace PetProject.Domain.Results;

public class Error
{
    public string Code { get; }
    public string Description { get; }

    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error IdRequired = new("IdRequired", "Id is required");
    public static readonly Error NameRequired = new("NameRequired", "Name is required");
    public static readonly Error NameTooShort = new("NameTooShort", "Name is too short");
    public static readonly Error NameTooLong = new("NameTooLong", "Name is too long");
    
}