using System.Text.RegularExpressions;
using PetProject.Domain.Consts;
using PetProject.Domain.Results;
using PetProject.Domain.Results.Errors;

namespace PetProject.Domain.Entities.ValueObjects;

public record SocialNetwork
{
    public string Title { get; private set; }
    public string Link { get; private set; }
    
    private SocialNetwork() { }
    private SocialNetwork(string title, string link)
    {
        Title = title;
        Link = link;
    }
    
    public static Result<SocialNetwork> Create(string title, string link)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result<SocialNetwork>.Failure(SocialNetworkErrors.TitleRequired);
        if (title.Length > SocialNetworkConsts.TitleMaxLength)
            return Result<SocialNetwork>.Failure(SocialNetworkErrors.TitleTooLong);
        if (string.IsNullOrWhiteSpace(link))
            return Result<SocialNetwork>.Failure(SocialNetworkErrors.LinkRequired);
        
        Regex regex = new Regex(@"^https?://[^\s/$.?#].[^\s]*$");
        if (!regex.IsMatch(link))
            return Result<SocialNetwork>.Failure(SocialNetworkErrors.LinkInvalid);
        
        
        return new SocialNetwork(title, link);
    }
}