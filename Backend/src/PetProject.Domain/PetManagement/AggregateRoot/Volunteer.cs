using PetProject.Domain.PetManagement.Entities;
using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.PetManagement.Enums;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.PetManagement.AggregateRoot;

public class Volunteer : Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    private Volunteer(VolunteerId id) : base(id) { }

    public FullName FullName { get; private set; }
    public NotNullableText Description { get; private set; }
    public Experience Experience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public VolunteerDetails Details { get; private set; }
    public IReadOnlyCollection<Pet> Pets => _pets;
    
    public Volunteer(
        VolunteerId id,
        FullName fullName,
        NotNullableText description,
        Experience experience,
        PhoneNumber phoneNumber,
        VolunteerDetails details,
        List<Pet>? pets)
        : base(id)
    {
        FullName = fullName;
        Description = description;
        PhoneNumber = phoneNumber;
        Details = details;
        Experience = experience;

        if (pets != null) AddPets(pets);
    }

    public void AddPets(List<Pet> pets) => _pets.AddRange(pets);
    public void AddSocialNetworks(List<SocialNetwork> socialNetworks) => Details.AddSocialNetworks(socialNetworks);
    public void AddRequisites(List<Requisite> requisites) => Details.AddRequisites(requisites);
    public int PetsLookingForHome() => _pets.Count(p => p.HelpStatus == HelpStatus.LookingForHome);
    public int PetsNeedsHelp() => _pets.Count(p => p.HelpStatus == HelpStatus.NeedsHelp);
    public int PetsFoundHome() => _pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);

}