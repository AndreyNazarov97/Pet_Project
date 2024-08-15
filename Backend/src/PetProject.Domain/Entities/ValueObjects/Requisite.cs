using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities.ValueObjects;

public class Requisite : ValueObject
{
    public string Title { get; }
    public string Description { get; }
    
    private Requisite(){}

    private Requisite(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public static Result<Requisite> Create(string title, string description)
    {
        if(string.IsNullOrWhiteSpace(title) || title.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Result<Requisite>.Failure(new("Invalid title", $"{nameof(title)} cannot be null or empty or longer than {Constants.MAX_SHORT_TEXT_LENGTH} characters"));
        if(string.IsNullOrWhiteSpace(description) || description.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Result<Requisite>.Failure(new("Invalid description", $"{nameof(description)} cannot be null or empty or longer than {Constants.MAX_LONG_TEXT_LENGTH} characters"));
        
        var requisite = new Requisite(title, description);
        return Result<Requisite>.Success(requisite);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Description;
    }
}
