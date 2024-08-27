﻿using System.Text.RegularExpressions;

namespace PetProject.Domain.Shared.ValueObjects;

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
            return Errors.General.ValueIsInvalid(nameof(number));

        var phoneNumber = new PhoneNumber(number);
        return Result<PhoneNumber>.Success(phoneNumber);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }

    public override string ToString()
    {
        return Number;
    }
}
