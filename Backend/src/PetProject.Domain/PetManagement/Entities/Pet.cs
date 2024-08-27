using PetProject.Domain.PetManagement.Entities.Details;
using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.PetManagement.Enums;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.PetManagement.Entities;

public class Pet : Entity<PetId>
{
    private readonly List<PetPhoto> _photos = [];

    private Pet(PetId id) : base(id) { }

    public Pet(
        PetId id,
        NotNullableString name,
        NotNullableText description,
        AnimalType animalType,
        NotNullableString breedName,
        NotNullableString color,
        NotNullableText healthInfo,
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

    public NotNullableString Name { get; private set; }
    public NotNullableText Description { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public NotNullableString BreedName { get; private set; }
    public NotNullableString Color { get; private set; }
    public NotNullableText HealthInfo { get; private set; }
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
    
    public void AddRequisites(List<Requisite> requisites) => Details.AddRequisites(requisites);
    public void AddPetPhotos(List<PetPhoto> petPhotos) => _photos.AddRange(petPhotos);
    
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
    
    public static Result<Pet> Create(
        PetId id,
        NotNullableString name,
        NotNullableText description,
        AnimalType animalType,
        NotNullableString breedName,
        NotNullableString color,
        NotNullableText healthInfo,
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