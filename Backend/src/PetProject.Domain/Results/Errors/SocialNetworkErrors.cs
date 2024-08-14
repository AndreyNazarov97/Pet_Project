namespace PetProject.Domain.Results.Errors;

public class SocialNetworkErrors : Error
{
    public SocialNetworkErrors(string code, string description) : base(code, description)
    {
    }
    
    public static SocialNetworkErrors TitleRequired => new(nameof(TitleRequired), "Title is required");
    public static SocialNetworkErrors TitleTooLong => new(nameof(TitleTooLong), "Title is too long");
    public static SocialNetworkErrors LinkRequired => new(nameof(LinkRequired), "Link is required");
    public static SocialNetworkErrors LinkInvalid => new(nameof(LinkInvalid), "Link is invalid");
 
}