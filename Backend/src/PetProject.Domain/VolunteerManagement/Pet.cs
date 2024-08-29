using CSharpFunctionalExtensions;
using PetProject.Domain.Interfaces;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement.Enums;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Domain.VolunteerManagement;

public class Pet : Shared.Common.Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;
    
    protected Pet(PetId id) : base(id){}

    public PetName PetName { get; private set; }
    public Description GeneralDescription { get; private set; }
    public Description HealthInformation { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public Address Address { get; private set; }
    public PetPhysicalAttributes PhysicalAttributes{ get; private set; } 
    public PhoneNumber PhoneNumber { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public bool IsCastrated { get; private set; }
    public bool IsVaccinated { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public DateTimeOffset DateCreated { get; }
    public PetPhotosList PetPhotosList { get; private set; }
    public RequisitesList RequisitesList { get; private set; }


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
        RequisitesList requisites,
        PetPhotosList petPhotos) : base(id)
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
        RequisitesList = requisites;
        PetPhotosList = petPhotos;
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