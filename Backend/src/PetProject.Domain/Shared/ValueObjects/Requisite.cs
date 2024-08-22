namespace PetProject.Domain.Shared.ValueObjects;

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
            return Errors.General.ValueIsRequired(nameof(title));
                
        if(string.IsNullOrWhiteSpace(description) || description.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return  Errors.General.ValueIsRequired(nameof(description));
        
        var requisite = new Requisite(title, description);
        return Result<Requisite>.Success(requisite);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Description;
    }
}
