using PetProject.Domain.Consts;
using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Enums;
using PetProject.Domain.Results;
using PetProject.Domain.Results.Errors;

namespace PetProject.Domain.Entities;

public class Pet : Entity
{
       
    public string Name { get; private set; }
    
    public string Type { get; private set; }
    
    public string Description { get; private set; }
    
    public Guid SpeciesId { get; private set; }
    
    public string BreedName { get; private set; }
    
    public string Color { get; private set; }
    
    public string HealthInfo { get; private set; }
    
    public Adress Address { get; private set; }
    
    public double Weight { get; private set; }
    
    public double Height { get; private set; }
    
    public PhoneNumber OwnerPhoneNumber { get; private set; }
    
    public bool IsCastrated { get; private set; }
    
    public DateTimeOffset BirthDate { get; private set; }
    
    public bool IsVaccinated { get; private set; }
    
    public HelpStatus HelpStatus { get; private set; }
    
    private readonly List<PetPhoto> _photos = [];
    public IReadOnlyCollection<PetPhoto> Photos => _photos;
    
    private readonly List<Requisite> _requisites = [];
    public IReadOnlyCollection<Requisite> Requisites => _requisites;
    
    public DateTimeOffset CreatedAt { get; private set; }

    #region Constructors
    private Pet()
    {
    }

    private Pet(string name, string type, string description, Guid speciesId, string breedName, string color, string healthInfo, Adress address, double weight, double height, PhoneNumber ownerPhoneNumber, bool isCastrated, DateTimeOffset birthDate, bool isVaccinated, HelpStatus helpStatus, DateTimeOffset createdAt)
    {
        Name = name;
        Type = type;
        Description = description;
        SpeciesId = speciesId;
        BreedName = breedName;
        Color = color;
        HealthInfo = healthInfo;
        Address = address;
        Weight = weight;
        Height = height;
        OwnerPhoneNumber = ownerPhoneNumber;
        IsCastrated = isCastrated;
        BirthDate = birthDate;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        CreatedAt = createdAt;
    }

    public static Result<Pet> Create(
        string name,
        string type,
        string description,
        Guid speciesId,
        string breedName,
        string color,
        string healthInfo,
        Adress address,
        double weight,
        double height,
        PhoneNumber ownerPhoneNumber,
        bool isCastrated,
        DateTimeOffset birthDate,
        bool isVaccinated,
        HelpStatus helpStatus,
        DateTimeOffset createdAt)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Pet>.Failure(Error.NameRequired);
        if (name.Length > PetConsts.NameMaxLength)
            return Result<Pet>.Failure(Error.NameTooLong);
        if (string.IsNullOrWhiteSpace(type))
            return Result<Pet>.Failure(PetErrors.TypeRequired);
        if (type.Length > PetConsts.TypeMaxLength)
            return Result<Pet>.Failure(PetErrors.TypeTooLong);
        if (string.IsNullOrWhiteSpace(description))
            return Result<Pet>.Failure(PetErrors.DescriptionRequired);
        if (description.Length > PetConsts.DescriptionMaxLength)
            return Result<Pet>.Failure(PetErrors.DescriptionTooLong);
        if (speciesId == Guid.Empty)
            return Result<Pet>.Failure(PetErrors.SpeciesIdRequired);
        if (string.IsNullOrWhiteSpace(breedName))
            return Result<Pet>.Failure(PetErrors.BreedNameRequired);
        if (breedName.Length > PetConsts.BreedNameMaxLength)
            return Result<Pet>.Failure(PetErrors.BreedNameTooLong);
        if (string.IsNullOrWhiteSpace(color))
            return Result<Pet>.Failure(PetErrors.ColorRequired);
        if (color.Length > PetConsts.ColorMaxLength)
            return Result<Pet>.Failure(PetErrors.ColorTooLong);
        if (string.IsNullOrWhiteSpace(healthInfo))
            return Result<Pet>.Failure(PetErrors.HealthInfoRequired);
        if (healthInfo.Length > PetConsts.HealthInfoMaxLength)
            return Result<Pet>.Failure(PetErrors.HealthInfoTooLong);
        if (weight <= 0)
            return Result<Pet>.Failure(PetErrors.WeightInvalid);
        if (height <= 0)
            return Result<Pet>.Failure(PetErrors.HeightInvalid);
        

        return new Pet(name, type, description, speciesId, breedName, color, healthInfo, address, weight, height, ownerPhoneNumber, isCastrated, birthDate, isVaccinated, helpStatus, createdAt);
    }
    #endregion
    
    public void AddRequisite(Requisite requisite) => _requisites.Add(requisite);

    public void AddPhoto(PetPhoto photo) => _photos.Add(photo);
}