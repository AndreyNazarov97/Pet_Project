using System.Text.RegularExpressions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities.ValueObjects;

public class FullName : ValueObject
{
    private static readonly Regex ValidationRegex = new Regex(
        @"^[\p{L}\p{M}]{2,50}$",
        RegexOptions.Singleline | RegexOptions.Compiled);
    public string FirstName { get; }
    public string LastName { get; }
    public string? Patronymic { get; }
    
    private FullName(){}

    private FullName(string firstName, string lastName, string? patronymic = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
    }

    public static Result<FullName> Create(string firstName, string lastName, string? patronymic = null)
    {
        if(string.IsNullOrWhiteSpace(firstName) || !ValidationRegex.IsMatch(firstName))
            return Result<FullName>.Failure(new("Invalid first name", $"{nameof(firstName)} cannot be null and cannot contain special characters"));
        if(string.IsNullOrWhiteSpace(lastName) || !ValidationRegex.IsMatch(lastName))
            return Result<FullName>.Failure(new("Invalid last name", $"{nameof(lastName)} cannot be null and cannot contain special characters"));
        if(patronymic != null && !ValidationRegex.IsMatch(patronymic))
            return Result<FullName>.Failure(new("Invalid patronymic", $"{nameof(patronymic)} cannot be null and cannot contain special characters"));
        
        var fullName = new FullName(firstName, lastName, patronymic);
        return Result<FullName>.Success(fullName);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        if (Patronymic != null) yield return Patronymic;
    }

    public override string ToString()
    {
        if(Patronymic == null)
            return $"{LastName} {FirstName} ";
        return $"{LastName} {FirstName} {Patronymic}";
    }
}