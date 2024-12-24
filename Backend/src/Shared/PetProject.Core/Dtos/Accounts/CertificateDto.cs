namespace PetProject.Core.Dtos.Accounts;

public record CertificateDto
{
    public required long Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Description { get; init; }
    
    public required DateTime IssuedAt { get; init; }
}