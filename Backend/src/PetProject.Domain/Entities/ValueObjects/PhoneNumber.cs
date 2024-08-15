using System.Text.RegularExpressions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities.ValueObjects;

public class PhoneNumber : ValueObject
{
    private static readonly Regex ValidationRegex = new Regex(
       @"^7\d{10}$",
       RegexOptions.Singleline | RegexOptions.Compiled);
    
    public string Number { get; }

    private PhoneNumber(){}

    private PhoneNumber(string number)
    {
        Number = number;
    }
    
    public static Result<PhoneNumber> Create(string number)
    {
        if(string.IsNullOrWhiteSpace(number) || !ValidationRegex.IsMatch(number))
            throw new Exception("Invalid phone number");

        var phoneNumber = new PhoneNumber(number);
        return Result<PhoneNumber>.Success(phoneNumber);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}
