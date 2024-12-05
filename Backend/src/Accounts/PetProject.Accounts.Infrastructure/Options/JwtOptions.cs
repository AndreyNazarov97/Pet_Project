namespace PetProject.Accounts.Infrastructure.Options;

public class JwtOptions
{
    public const string Jwt = nameof(Jwt);
    
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecretKey { get; init; }
    public int ExpirationMinutes { get; init; }
}