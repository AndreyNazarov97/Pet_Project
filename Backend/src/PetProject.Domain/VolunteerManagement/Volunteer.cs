using PetProject.Domain.Interfaces;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.Common;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement.Enums;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Domain.VolunteerManagement;

public class Volunteer : AggregateRoot<VolunteerId>, ISoftDeletable
{ 
    private bool _isDeleted;
    private readonly List<Pet> _pets = [];
    
    private Volunteer(VolunteerId id) : base(id){}

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description generalDescription,
        Experience experience,
        PhoneNumber number,
        SocialLinksList socialLinksList,
        RequisitesList requisitesList) : base(id)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        Experience = experience;
        PhoneNumber = number;
        SocialLinksList = socialLinksList;
        RequisitesList = requisitesList;
    }

    public FullName FullName { get; private set; }
    public Description GeneralDescription { get; private set; }
    public Experience Experience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<Pet>? Pets => _pets.AsReadOnly();
    public SocialLinksList SocialLinksList { get; private set; }
    public RequisitesList RequisitesList { get; private set; }

    public void AddPet(Pet pet) => _pets.Add(pet);
    public int PetsAdoptedCount() =>
        _pets.Count(x => x.HelpStatus == HelpStatus.FoundHome);

    public int PetsFoundHomeQuantity() =>
        _pets.Count(x => x.HelpStatus == HelpStatus.LookingForHome);

    public int PetsUnderTreatmentCount() =>
        _pets.Count(x => x.HelpStatus == HelpStatus.NeedsHelp);
    
    public void UpdateMainInfo(FullName fullName,
        Description generalDescription,
        Experience experience,
        PhoneNumber number)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        Experience = experience;
        PhoneNumber = number;
    }
    public void UpdateSocialLinks(SocialLinksList list) =>
        SocialLinksList = list;
    public void UpdateRequisites(RequisitesList list) =>
        RequisitesList = list;
    
    public Pet? GetById(PetId id) => _pets.FirstOrDefault(x => x.Id == id);

    public void Activate()
    {
        _isDeleted = false;
        foreach (var pet in _pets)
        {
            pet.Activate();
        }
    }
    public void Deactivate()
    {
        _isDeleted = true;
        foreach (var pet in _pets)
        {
            pet.Deactivate();
        }
    }
}