using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Enums;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class Pet : Entity<PetId>
{
    private readonly List<PetPhoto> _photos = [];
    private readonly List<Requisite> _requisites = [];
    
    private Pet(){}

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
        List<Requisite> requisites,
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
        AddRequisites(requisites);
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
    
    public IReadOnlyCollection<Requisite> Requisites => _requisites.AsReadOnly();
    public IReadOnlyCollection<PetPhoto> Photos => _photos.AsReadOnly();

    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);
    public void AddPetPhotos(List<PetPhoto> petPhotos) => _photos.AddRange(petPhotos);
    
    public void SetVaccinated(bool isVaccinated) => IsVaccinated = isVaccinated;
    public void SetCastrated(bool isCastrated) => IsCastrated = isCastrated;

    public Result SetMainPhoto(PetPhoto photo)
    {
        if (!_photos.Contains(photo))
        {
            return Result.Failure(new Error("Pet photo not found", "The photo you want to set is not in your list of photos."));
        }
        else
        {
            _photos.ForEach(x => x.SetAsNotMain());
            photo.SetAsMain();
            return Result.Success();
        }
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
        List<Requisite>? requisites,
        List<PetPhoto>? photos)
    {
        if(string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Result<Pet>.Failure(new("Invalid name", $"{nameof(name)} cannot be null or empty or longer than {Constants.MAX_SHORT_TEXT_LENGTH} characters."));
        
        if(string.IsNullOrWhiteSpace(description) || description.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Result<Pet>.Failure(new("Invalid description", $"{nameof(description)} cannot be null or empty or longer than {Constants.MAX_LONG_TEXT_LENGTH} characters."));

        if(string.IsNullOrWhiteSpace(breedName) || breedName.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Result<Pet>.Failure(new("Invalid breedName", $"{nameof(breedName)} cannot be null or empty or longer than {Constants.MAX_SHORT_TEXT_LENGTH} characters."));

        if(string.IsNullOrWhiteSpace(color) || color.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Result<Pet>.Failure(new("Invalid color", $"{nameof(color)} cannot be null or empty or longer than {Constants.MAX_SHORT_TEXT_LENGTH} characters."));

        if(string.IsNullOrWhiteSpace(healthInfo) || healthInfo.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Result<Pet>.Failure(new("Invalid healthInfo", $"{nameof(healthInfo)} cannot be null or empty or longer than {Constants.MAX_LONG_TEXT_LENGTH} characters."));
         
        if(weight < Constants.MIN_VALUE)
            return Result<Pet>.Failure(new("Invalid weight", $"{nameof(weight)} cannot be less than {Constants.MIN_VALUE}."));

        if(height < Constants.MIN_VALUE)
            return Result<Pet>.Failure(new("Invalid height", $"{nameof(height)} cannot be less than {Constants.MIN_VALUE}."));

        if(birthDate > DateTimeOffset.UtcNow)
            return Result<Pet>.Failure(new("Invalid birthDate", $"{nameof(birthDate)} cannot be greater than {DateTimeOffset.UtcNow}."));
        
        
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
            requisites ?? [],
            photos ?? []);

        return Result<Pet>.Success(pet);
    }
}