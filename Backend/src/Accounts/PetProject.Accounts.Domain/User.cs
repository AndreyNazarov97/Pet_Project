using Microsoft.AspNetCore.Identity;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Domain;

public class User : IdentityUser<long>
{
    private List<SocialNetwork> _socialNetworks = [];
    private List<Photo> _photos = [];
    
    public long? AdminAccountId { get; set; }
    public AdminAccount? AdminAccount { get; set; }
    
    public long? ParticipantAccountId { get; set; }
    public ParticipantAccount? ParticipantAccount { get; set; }
    
    public long? VolunteerAccountId { get; set; }
    public VolunteerAccount? VolunteerAccount { get; set; }
    
    public FullName FullName { get; set; }
    
    public List<Role> Roles { get; set; } = [];
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks.AsReadOnly();
    public IReadOnlyList<Photo> Photos => _photos.AsReadOnly();
   
}
