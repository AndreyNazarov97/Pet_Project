namespace PetProject.Domain.Results.Errors;

public class PhoneNumberError : Error
{
    public PhoneNumberError(string code, string description) : base(code, description)
    {
    }
    
    public static PhoneNumberError PhoneNumberRequired => new PhoneNumberError("PhoneNumberRequired", "Phone number is required");
    public static PhoneNumberError PhoneNumberInvalid => new PhoneNumberError("PhoneNumberInvalid", "Phone number is invalid");
}