using PetProject.Domain.Shared;

namespace PetProject.Domain.PetManagement.Entities.ValueObjects;

public record SocialNetwork 
{
    private SocialNetwork(string title, string link)
    {
        Title = title;
        Link = link;
    }

    public string Title { get; }
    public string Link { get; }

    public static Result<SocialNetwork> Create(string title, string link)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(title));
        if (string.IsNullOrWhiteSpace(link) || link.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(link));

        var socialNetwork = new SocialNetwork(title, link);
        return socialNetwork;
    }

    public override string ToString()
    {
        return $"{Title} - {Link}";
    }
}