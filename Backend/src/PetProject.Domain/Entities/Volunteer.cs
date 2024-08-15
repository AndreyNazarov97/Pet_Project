using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class Volunteer : Entity<VolunteerId>
{
    private readonly List<Requisite> _requisites = [];
    private readonly List<SocialNetwork> _socialNetworks = [];

    private Volunteer()
    {
    }

    private Volunteer(
        VolunteerId id,
        FullName fullName,
        string description,
        int experience,
        int petsAdopted,
        int petsFoundHomeQuantity,
        int petsInTreatment,
        PhoneNumber phoneNumber,
        List<SocialNetwork> socialNetworks,
        List<Requisite> requisites)
        : base(id)
    {
        FullName = fullName;
        Description = description;
        Experience = experience;
        PetsAdopted = petsAdopted;
        PetsFoundHomeQuantity = petsFoundHomeQuantity;
        PetsInTreatment = petsInTreatment;
        PhoneNumber = phoneNumber;

        AddRequisites(requisites);
        AddSocialNetworks(socialNetworks);
    }

    public FullName FullName { get; private set; }
    public string Description { get; private set; }
    public int Experience { get; private set; }
    public int PetsAdopted { get; private set; }
    public int PetsFoundHomeQuantity { get; private set; }
    public int PetsInTreatment { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    public IReadOnlyCollection<SocialNetwork> SocialNetworks => _socialNetworks;
    public IReadOnlyCollection<Requisite> Requisites => _requisites;

    public void AddSocialNetworks(List<SocialNetwork> socialNetworks) => _socialNetworks.AddRange(socialNetworks);
    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);

    //TODO добавить логику подсчета животных

    public static Result<Volunteer> Create(
        VolunteerId volunteerId,
        string firstName,
        string lastName,
        string? patronymic,
        string phoneNumber,
        string description,
        int experience,
        int petsAdopted,
        int petsFoundHomeQuantity,
        int petsInTreatment,
        List<SocialNetwork>? socialNetworks,
        List<Requisite>? requisites
    )
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Result<Volunteer>.Failure(new("Invalid description",
                $"{nameof(description)} cannot be null or empty or longer than {Constants.MAX_LONG_TEXT_LENGTH} characters."));

        if (experience < Constants.MIN_VALUE)
            return Result<Volunteer>.Failure(new("Invalid experience",
                $"{nameof(experience)} cannot be less than {Constants.MIN_VALUE}"));

        if (petsAdopted < Constants.MIN_VALUE)
            return Result<Volunteer>.Failure(new("Invalid petsAdopted",
                $"{nameof(petsAdopted)} cannot be less than {Constants.MIN_VALUE}"));

        if (petsFoundHomeQuantity < Constants.MIN_VALUE)
            return Result<Volunteer>.Failure(new("Invalid petsFoundHomeQuantity",
                $"{nameof(petsFoundHomeQuantity)} cannot be less than {Constants.MIN_VALUE}"));

        if (petsInTreatment < Constants.MIN_VALUE)
            return Result<Volunteer>.Failure(new("Invalid petsInTreatment",
                $"{nameof(petsInTreatment)} cannot be less than {Constants.MIN_VALUE}"));
        
        var fullName = FullName.Create(firstName, lastName, patronymic);
        if(fullName.IsFailure)
            return Result<Volunteer>.Failure(fullName.Error!);

        var phoneNumberValue = PhoneNumber.Create(phoneNumber);
        if (phoneNumberValue.IsFailure)
            return Result<Volunteer>.Failure(phoneNumberValue.Error!);
        
        var volunteer = new Volunteer(
            volunteerId,
            fullName.Value,
            description,
            experience,
            petsAdopted,
            petsFoundHomeQuantity,
            petsInTreatment,
            phoneNumberValue.Value,
            socialNetworks ?? [],
            requisites ?? []
        );
        
        return Result<Volunteer>.Success(volunteer);
    }
}