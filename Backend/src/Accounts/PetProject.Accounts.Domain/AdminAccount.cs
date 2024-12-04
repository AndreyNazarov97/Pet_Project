using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Domain;

public class AdminAccount
{
    private AdminAccount() {}
    
    public AdminAccount(
        User user)
    {
        User = user;
        UserId = user.Id;
    }
    
    public long Id { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}