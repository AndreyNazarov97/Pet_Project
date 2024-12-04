using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Domain;

public class VolunteerAccount
{
    private List<Requisite> _requisites = [];
    
    private VolunteerAccount() {}
    
    public VolunteerAccount(
        Experience experience,
        User user)
    {
        Experience = experience;
        User = user;
        UserId = user.Id;
    }
    
    public long Id { get; set; }
    
    public Experience Experience { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public IReadOnlyList<Requisite> Requisites => _requisites.AsReadOnly();
    public List<Certificate> Certificates { get; set; } = [];
}