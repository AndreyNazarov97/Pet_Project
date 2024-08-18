using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Enums;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class Volunteer : Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    private Volunteer()
    {
    }

    private Volunteer(
        VolunteerId id,
        FullName fullName,
        string description,
        int experience,
        PhoneNumber phoneNumber,
        VolunteerDetails details,
        List<Pet> pets)
        : base(id)
    {
        FullName = fullName;
        Description = description;
        PhoneNumber = phoneNumber;
        Details = details;
        Experience = experience;

        AddPets(pets);
    }

    public FullName FullName { get; private set; }
    public string Description { get; private set; }

    public int Experience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public VolunteerDetails Details { get; private set; }
    public IReadOnlyCollection<Pet> Pets => _pets;


    public void AddPets(List<Pet> pets) => _pets.AddRange(pets);
    public void UpdateDetails(VolunteerDetails details) => Details = details;
    public void AddSocialNetworks(List<SocialNetwork> socialNetworks) => Details.AddSocialNetworks(socialNetworks);
    public void AddRequisites(List<Requisite> requisites) => Details.AddRequisites(requisites);
    public int PetsLookingForHome() => _pets.Count(p => p.HelpStatus == HelpStatus.LookingForHome);
    public int PetsNeedsHelp() => _pets.Count(p => p.HelpStatus == HelpStatus.NeedsHelp);
    public int PetsFoundHome() => _pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);

    public static Result<Volunteer> Create(
        VolunteerId volunteerId,
        FullName fullName,
        string description,
        int experience,
        PhoneNumber phoneNumber,
        VolunteerDetails details,
        List<Pet>? pets
    )
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(description));

        if (experience < Constants.MIN_VALUE)
            return Errors.General.ValueIsInvalid(nameof(experience));

        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            description,
            experience,
            phoneNumber,
            details,
            pets ?? []
        );

        return Result<Volunteer>.Success(volunteer);
    }
}