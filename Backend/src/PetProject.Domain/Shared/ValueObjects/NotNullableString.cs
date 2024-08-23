namespace PetProject.Domain.Shared.ValueObjects;
/// <summary>
/// ValueObject для коротких строк
/// </summary>
public class NotNullableString : ValueObject
{
    private NotNullableString(){}
    
    private NotNullableString(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<NotNullableString> Create(string text)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(text));
        
        return Result<NotNullableString>.Success(new NotNullableString(text));
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