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
    public RequisitesList Requisites { get; private set; } 
    public SocialNetworkList SocialNetworks { get; private set; }
    public IReadOnlyCollection<Pet> Pets => _pets.AsReadOnly();
    
    public Volunteer(
        VolunteerId id,
        FullName fullName,
        NotNullableText description,
        Experience experience,
        PhoneNumber phoneNumber,
        RequisitesList requisites,
        SocialNetworkList socialNetworks)
        : base(id)
    {
        FullName = fullName;
        Description = description;
        PhoneNumber = phoneNumber;
        Experience = experience;
        Requisites = requisites;
        SocialNetworks = socialNetworks;
    }

    public void AddPet(Pet pet) => _pets.Add(pet);
    public int PetsLookingForHome() => _pets.Count(p => p.HelpStatus == HelpStatus.LookingForHome);
    public int PetsNeedsHelp() => _pets.Count(p => p.HelpStatus == HelpStatus.NeedsHelp);
    public int PetsFoundHome() => _pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);

    public void UpdateMainInfo(
        FullName? fullName, PhoneNumber? phoneNumber, NotNullableText? description, Experience? experience)
    {
        FullName = fullName ?? FullName;
        PhoneNumber = phoneNumber ?? PhoneNumber;
        Description = description ?? Description;
        Experience = experience ?? Experience;
    }

    public void UpdateSocialNetworks(SocialNetworkList socialNetworks) => SocialNetworks = socialNetworks;
    public void UpdateRequisites(RequisitesList requisites) => Requisites = requisites;
}