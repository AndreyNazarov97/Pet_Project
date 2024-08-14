namespace PetProject.Domain.Results.Errors;

public class FullNameErrors : Error
{
    public FullNameErrors(string code, string description) : base(code, description)
    {
    }
    
    public static Error FirstNameRequired => new FullNameErrors(nameof(FirstNameRequired), "First name is required");
    public static Error FirstNameTooLong => new FullNameErrors(nameof(FirstNameTooLong), "First name is too long");
    public static Error FirstNameTooShort => new FullNameErrors(nameof(FirstNameTooShort), "First name is too short");
    public static Error LastNameRequired => new FullNameErrors(nameof(LastNameRequired), "Last name is required");
    public static Error LastNameTooLong => new FullNameErrors(nameof(LastNameTooLong), "Last name is too long");
    public static Error LastNameTooShort => new FullNameErrors(nameof(LastNameTooShort), "Last name is too short");
    public static Error PatronymicTooLong => new FullNameErrors(nameof(PatronymicTooLong), "Patronymic is too long");
    public static Error PatronymicTooShort => new FullNameErrors(nameof(PatronymicTooShort), "Patronymic is too short");
}