namespace PetProject.Core.Dtos.Accounts;

public record VolunteerAccountDto
{
    public required long VolunteerAccountId { get; init; }
    
    public RequisiteDto[] Requisites { get; set; } = [];
    
    public int Experience { get; init; }
    
    public CertificateDto[] Certificates { get; init; } = [];
}