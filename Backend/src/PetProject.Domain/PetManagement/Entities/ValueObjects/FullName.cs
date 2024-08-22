using System.Text.RegularExpressions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.PetManagement.Entities.ValueObjects;

public class FullName : ValueObject
{
    private static readonly Regex ValidationRegex = new Regex(
        @"^[\p{L}\p{M}]{2,50}$",
        RegexOptions.Singleline | RegexOptions.Compiled);
    
    private FullName(){}

    private FullName(string firstName, string lastName, string? patronymic = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
    }
    
    public string FirstName { get; }
    public string LastName { get; }
    public string? Patronymic { get; }

    public static Result<FullName> Create(string firstName, string lastName, string? patronymic = null)
    {
        if (string.IsNullOrWhiteSpace(firstName) || !ValidationRegex.IsMatch(firstName))
            return Errors.General.ValueIsInvalid(nameof(firstName));
        
        if(string.IsNullOrWhiteSpace(lastName) || !ValidationRegex.IsMatch(lastName))
            return Errors.General.ValueIsInvalid(nameof(lastName));
        
        if(patronymic != null && !ValidationRegex.IsMatch(patronymic))
            return Errors.General.ValueIsInvalid(nameof(patronymic));
        
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