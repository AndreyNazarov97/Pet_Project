namespace PetProject.Accounts.Domain;

public class RefreshSession
{
    public Guid Id { get; init; }
    public long UserId { get; init; }
    public User User { get; init; } = null!;
    public Guid RefreshToken { get; init; }
    public Guid Jti { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}