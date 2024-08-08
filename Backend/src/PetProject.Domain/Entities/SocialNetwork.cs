namespace PetProject.Domain.Entities;

public class SocialNetwork
{
    public Guid Id { get; }
    public string Title { get; }
    public string Link { get; }
    private SocialNetwork()
    {
        
    }
}