namespace PetProject.Core.Dtos.Accounts;

public record UserDto
{
    public required long Id { get; init; }
    
    public required string UserName { get; init; }
    
    public required string Email { get; init; }
    
    public required FullNameDto FullName { get; set; }

    public RoleDto[] Roles { get; set; } = [];
    
    public VolunteerAccountDto? VolunteerAccount { get; set; }
    
    public ParticipantAccountDto? ParticipantAccount { get; set; }
    
    public SocialNetworkDto[] SocialNetworks { get; set; } = [];
    
    public PhotoDto[] Photos { get; set; } = [];
}