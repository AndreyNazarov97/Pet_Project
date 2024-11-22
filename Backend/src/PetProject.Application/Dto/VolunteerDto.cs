namespace PetProject.Application.Dto;

public record VolunteerDto
{
    public required FullNameDto FullName { get; init; }
    public required string GeneralDescription { get; init; }
    public required int AgeExperience { get; init; }
    public required string PhoneNumber { get; init; } 
    public RequisiteDto[] Requisites { get; init; } = [];
    public SocialLinkDto[] SocialLinks { get; init; } = [];
    public PetDto[] Pets { get; private set; } = [];
    
    public void AddPet(PetDto pet)
    {
        Pets = Pets.Append(pet).ToArray();
    }
}