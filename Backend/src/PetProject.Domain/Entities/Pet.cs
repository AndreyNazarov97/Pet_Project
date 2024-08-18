using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Enums;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class Pet : Entity<PetId>
{
    private readonly List<PetPhoto> _photos = [];

    private Pet()
    {
    }

    public Pet(
        PetId id,
        string name,
        string description,
        AnimalType animalType,
        string breedName,
        string color,
        string healthInfo,
        Adress address,
        double weight,
        double height,
        PhoneNumber ownerPhoneNumber,
        bool isCastrated,
        bool isVaccinated,
        DateTimeOffset birthDate,
        HelpStatus helpStatus,
        PetDetails details,
        List<PetPhoto> photos)
        : base(id)
    {
        Name = name;
        Description = description;
        AnimalType = animalType;
        BreedName = breedName;
        Color = color;
        HealthInfo = healthInfo;
        Address = address;
        Weight = weight;
        Height = height;
        OwnerPhoneNumber = ownerPhoneNumber;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        BirthDate = birthDate;
        HelpStatus = helpStatus;
        CreatedAt = DateTimeOffset.UtcNow;
        Details = details;
        AddPetPhotos(photos);
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public string BreedName { get; private set; }
    public string Color { get; private set; }
    public string HealthInfo { get; private set; }
    public Adress Address { get; private set; }
    public double Weight { get; private set; }
    public double Height { get; private set; }
    public PhoneNumber OwnerPhoneNumber { get; private set; }
    public bool IsCastrated { get; private set; }
    public bool IsVaccinated { get; private set; }
    public DateTimeOffset BirthDate { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public PetDetails Details { get; private set; }


    public IReadOnlyCollection<PetPhoto> Photos => _photos.AsReadOnly();

    public void UpdateDetails(PetDetails details) => Details = details;
    public void AddRequisites(List<Requisite> requisites) => Details.AddRequisites(requisites);
    public void AddPetPhotos(List<PetPhoto> petPhotos) => _photos.AddRange(petPhotos);

    public void SetVaccinated(bool isVaccinated) => IsVaccinated = isVaccinated;
    public void SetCastrated(bool isCastrated) => IsCastrated = isCastrated;

    public Result SetMainPhoto(PetPhoto photo)
    {
        if (!_photos.Contains(photo))
        {
            return Errors.General.NotFound();
        }
        
        _photos.ForEach(x => x.SetAsNotMain());
        photo.SetAsMain();
        return Result.Success();
    }

    public Result UpdatePhoneNumber(PhoneNumber phoneNumber)
    {
        OwnerPhoneNumber = phoneNumber;
        return Result.Success();
    }

    public Result UpdateHelpStatus(HelpStatus helpStatus)
    {
        HelpStatus = helpStatus;
        return Result.Success();
    }

    public Result UpdateAdress(Adress adress)
    {
        Address = adress;
        return Result.Success();
    }

    public static Result<Pet> Create(
        PetId id,
        string name,
        string description,
        AnimalType animalType,
        string breedName,
        string color,
        string healthInfo,
        Adress address,
        double weight,
        double height,
        PhoneNumber ownerPhoneNumber,
        bool isCastrated,
        bool isVaccinated,
        DateTimeOffset birthDate,
        HelpStatus helpStatus,
        PetDetails details,
        List<PetPhoto>? photos)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(name));
                
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(description));
        
        if (string.IsNullOrWhiteSpace(breedName) || breedName.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(breedName));
        
        if (string.IsNullOrWhiteSpace(color) || color.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(color));
        
        if (string.IsNullOrWhiteSpace(healthInfo) || healthInfo.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(healthInfo));
        
        if (weight < Constants.MIN_VALUE)
            return Errors.General.ValueIsInvalid(nameof(weight));
        
        if (height < Constants.MIN_VALUE)
            return Errors.General.ValueIsInvalid(nameof(height));
        
        if (birthDate > DateTimeOffset.UtcNow)
            return Errors.General.ValueIsInvalid(nameof(birthDate));

        var pet = new Pet(
            id,
            name,
            description,
            animalType,
            breedName,
            color,
            healthInfo,
            address,
            weight,
            height,
            ownerPhoneNumber,
            isCastrated,
            isVaccinated,
            birthDate,
            helpStatus,
            details,
            photos ?? []);

        return Result<Pet>.Success(pet);
    }
}