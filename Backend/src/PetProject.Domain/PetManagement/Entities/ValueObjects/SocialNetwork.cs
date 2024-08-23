using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.PetManagement.Entities.ValueObjects;

public class SocialNetwork : ValueObject
{
    private SocialNetwork(){}

    private SocialNetwork(string title, string link)
    {
        Title = title;
        Link = link;
    }
    
    public string Title { get; }
    public string Link { get; }

    public static Result<SocialNetwork> Create(string title, string link)
    {
        if(string.IsNullOrWhiteSpace(title) || title.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(title));
        if (string.IsNullOrWhiteSpace(link) || link.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(link));
        
        var socialNetwork = new SocialNetwork(title, link);
        return Result<SocialNetwork>.Success(socialNetwork);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Link;
    }

    public override string ToString()
    {
        return $"{Title} - {Link}";
    }
}