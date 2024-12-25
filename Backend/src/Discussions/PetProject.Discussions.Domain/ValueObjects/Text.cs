using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Domain.ValueObjects;

public record Text
{
    public string Value { get; }

    private Text(string value)
    {
        Value = value;
    }

    public static Result<Text, Error> Create(string text)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length > Constants.MAX_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Description");

        return new Text(text);
    }
}