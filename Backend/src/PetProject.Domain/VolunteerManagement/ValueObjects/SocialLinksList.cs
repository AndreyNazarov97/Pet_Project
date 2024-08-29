namespace PetProject.Domain.VolunteerManagement.ValueObjects;

public record SocialLinksList
{
    private SocialLinksList() { }
    public IReadOnlyCollection<SocialLink> SocialLinks { get; }
    public SocialLinksList(IEnumerable<SocialLink> socialLinks) => SocialLinks = socialLinks.ToList();
}