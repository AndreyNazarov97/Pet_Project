using Microsoft.AspNetCore.Identity;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Domain;

public class User : IdentityUser<long>
{
    private List<SocialNetwork> _socialNetworks = [];
    private List<Photo> _photos = [];
    private List<Role> _roles = [];
    
    public long? AdminAccountId { get; set; }
    public AdminAccount? AdminAccount { get; set; }
    
    public long? ParticipantAccountId { get; set; }
    public ParticipantAccount? ParticipantAccount { get; set; }
    
    public long? VolunteerAccountId { get; set; }
    public VolunteerAccount? VolunteerAccount { get; set; }

    public FullName FullName { get; set; } = null!;
    
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks.AsReadOnly();
    public IReadOnlyList<Photo> Photos => _photos.AsReadOnly();

    public void AddSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
    {
        _socialNetworks.AddRange(socialNetworks);
    }
    
    public static User CreateAdmin(FullName fullName,
        string userName, string email, Role role)
    {
        var user = new User()
        {
            FullName = fullName,
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            _roles = [role]
        };
        
        return user;
    }
    
    public static User CreateParticipant(FullName fullName,
        string userName, string email, Role role)
    {
        var user = new User()
        {
            FullName = fullName,
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            _roles = [role]
        };
        
        return user;
    }

}
