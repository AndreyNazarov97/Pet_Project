using CSharpFunctionalExtensions;

namespace PetProject.SharedKernel.Shared.ValueObjects;

public record SpeciesName
{
    public string Value { get; }

    private SpeciesName(string value)
    {
        Value = value;
    }

    public static Result<SpeciesName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("BreedName");

        return new SpeciesName(value);
    }
    
    
}