using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Domain;

public class ParticipantAccount
{
    public const string Participant = nameof(Participant);
    
    private ParticipantAccount() {}
    
    public ParticipantAccount(User user)
    {
        User = user;
        UserId = user.Id;
    }
    
    public long Id { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}