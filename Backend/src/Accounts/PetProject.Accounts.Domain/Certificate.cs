namespace PetProject.Accounts.Domain;

public class Certificate
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime IssuedAt { get; set; }
}