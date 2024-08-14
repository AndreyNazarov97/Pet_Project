using System.Text.RegularExpressions;
using PetProject.Domain.Results;
using PetProject.Domain.Results.Errors;

namespace PetProject.Domain.Entities.ValueObjects;

public record PhoneNumber
{
    public string Number { get; private set; }
    
    private PhoneNumber(){}
    private PhoneNumber(string number)
    {
        Number = number;
    }

    public static Result<PhoneNumber> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return Result<PhoneNumber>.Failure(PhoneNumberError.PhoneNumberRequired);
        
        Regex regex = new Regex(@"^7\d{10}$");
        if (!regex.IsMatch(number))
            return Result<PhoneNumber>.Failure(PhoneNumberError.PhoneNumberInvalid);
        
        return new PhoneNumber(number);
    }
}
