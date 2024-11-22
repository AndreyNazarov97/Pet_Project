using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Dto;

public record VolunteerDto
{
    public required Guid Id { get; init; }
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

    public Volunteer ToEntity()
    {
        var volunteer = new Volunteer(
            VolunteerId.Create(Id), 
            FullName.ToEntity(), 
            Description.Create(GeneralDescription).Value, 
            Experience.Create(AgeExperience).Value, 
            Domain.Shared.ValueObjects.PhoneNumber.Create(PhoneNumber).Value, 
            new SocialLinksList(SocialLinks.Select(s => s.ToEntity()).ToArray()) , 
            new RequisitesList(Requisites.Select(r => r.ToEntity()).ToArray()) 
            );
        
        foreach (var pet in Pets)
        {
            volunteer.AddPet(pet.ToEntity());
        }
        
        return volunteer;
    }
}