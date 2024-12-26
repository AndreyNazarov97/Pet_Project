namespace PetProject.Core.Dtos.VolunteerRequests;

public record VolunteerInfoDto
{
    public required FullNameDto FullName { get; set; }
    public required string PhoneNumber { get; init; }
    public required int WorkExperience { get; init; }
    public required string GeneralDescription { get; init; }
    public SocialNetworkDto[] SocialNetworks { get; set; } = [];
}