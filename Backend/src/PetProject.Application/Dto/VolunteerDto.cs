namespace PetProject.Application.Dto;

public record VolunteerDto
{
    public string FullName { get; init; } = null!;
    public string GeneralDescription { get; init; } = null!;
    public int AgeExperience { get; init; }
    public string PhoneNumber { get; init; } = null!;
}