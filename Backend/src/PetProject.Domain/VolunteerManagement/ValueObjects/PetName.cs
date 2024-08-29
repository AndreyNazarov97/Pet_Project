using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.VolunteerManagement.ValueObjects;

public record PetName
{
    public string Value { get; }

    private PetName(string value)
    {
        Value = value;
    }

    public static Result<PetName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Name");

        return new PetName(value);
    }
}