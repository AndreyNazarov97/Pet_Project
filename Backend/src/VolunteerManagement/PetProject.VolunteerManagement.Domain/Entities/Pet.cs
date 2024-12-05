using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Interfaces;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Domain.Enums;
using PetProject.VolunteerManagement.Domain.ValueObjects;

namespace PetProject.VolunteerManagement.Domain.Entities;

public class Pet : SharedKernel.Shared.Common.Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;
    private List<PetPhoto> _PetPhotoList = [];

    protected Pet(PetId id) : base(id)
    {
    }

    public PetName PetName { get; private set; }
    public Description GeneralDescription { get; private set; }
    public Description HealthInformation { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public Address Address { get; private set; }
    public PetPhysicalAttributes PhysicalAttributes { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public bool IsCastrated { get; private set; }
    public bool IsVaccinated { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public Position Position { get; private set; }
    public DateTimeOffset DateCreated { get; }
    public IReadOnlyList<PetPhoto> PetPhotoList => _PetPhotoList.AsReadOnly();


    public Pet(PetId id,
        PetName petName,
        Description generalDescription,
        Description healthInformation,
        AnimalType animalType,
        Address address,
        PetPhysicalAttributes attributes,
        PhoneNumber number,
        DateOnly birthDate,
        bool isCastrated,
        bool isVaccinated,
        HelpStatus helpStatus,
        List<PetPhoto> petPhoto) : base(id)
    {
        PetName = petName;
        GeneralDescription = generalDescription;
        HealthInformation = healthInformation;
        AnimalType = animalType;
        Address = address;
        PhysicalAttributes = attributes;
        PhoneNumber = number;
        BirthDate = birthDate;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        _PetPhotoList = petPhoto;
        DateCreated = DateTimeOffset.UtcNow;
    }

    public void UpdatePet(
        PetName? petName,
        Description? generalDescription,
        Description? healthInformation,
        AnimalType? animalType,
        Address? address,
        PetPhysicalAttributes? attributes,
        DateOnly? birthDate,
        bool? isCastrated,
        bool? isVaccinated,
        HelpStatus? helpStatus,
        List<Requisite>? requisites)
    {
        PetName = petName ?? PetName;
        GeneralDescription = generalDescription ?? GeneralDescription;
        HealthInformation = healthInformation ?? HealthInformation;
        AnimalType = animalType ?? AnimalType;
        Address = address ?? Address;
        PhysicalAttributes = attributes ?? PhysicalAttributes;
        BirthDate = birthDate ?? BirthDate;
        IsCastrated = isCastrated ?? IsCastrated;
        IsVaccinated = isVaccinated ?? IsVaccinated;
        HelpStatus = helpStatus ?? HelpStatus;
    }

    public void AddPhotos(IEnumerable<PetPhoto> petPhotos)
    {
        _PetPhotoList.AddRange(petPhotos);
    }

    public void SetPosition(Position position)
    {
        Position = position;
    }

    public void ChangeStatus(HelpStatus status)
    {
        HelpStatus = status;
    }

    public UnitResult<Error> SetMainPhoto(PetPhoto petPhoto)
    {
        var isPhotoExist = PetPhotoList.FirstOrDefault(p => p.Path == petPhoto.Path);
        if (isPhotoExist is null)
            return Errors.General.NotFound();

        _PetPhotoList = PetPhotoList
            .Select(photo => new PetPhoto(photo.Path) { IsMain = photo.Path == petPhoto.Path })
            .OrderByDescending(p => p.IsMain)
            .ToList();

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> DeletePhoto(FilePath filePath)
    {
        var isPhotoExist = PetPhotoList.FirstOrDefault(p => p.Path == filePath);
        if (isPhotoExist is null)
            return Errors.General.NotFound();

        _PetPhotoList.Remove(isPhotoExist);
        if (isPhotoExist.IsMain && _PetPhotoList.Count > 0)
        {
            _PetPhotoList[0] = new PetPhoto(_PetPhotoList[0].Path) { IsMain = true };
        }

        return UnitResult.Success<Error>();
    }

    public void Activate()
    {
        _isDeleted = false;
    }

    public void Deactivate()
    {
        _isDeleted = true;
    }
}