namespace PetProject.Domain.Shared.ValueObjects;

/// <summary>
/// ValueObject для текстовых полей
/// </summary>
public class NotNullableText : ValueObject
{
    private NotNullableText(){}

    private NotNullableText(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<NotNullableText> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(description));

        return Result<NotNullableText>.Success(new NotNullableText(description));
    }
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}