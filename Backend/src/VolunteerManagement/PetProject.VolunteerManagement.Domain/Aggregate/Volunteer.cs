using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Interfaces;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.Common;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Domain.Entities;
using PetProject.VolunteerManagement.Domain.Enums;
using PetProject.VolunteerManagement.Domain.ValueObjects;

namespace PetProject.VolunteerManagement.Domain.Aggregate;

public class Volunteer : AggregateRoot<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted;
    private readonly List<Pet> _pets = [];
    private List<Requisite> _requisites = [];
    private List<SocialLink> _socialLinks = [];

    private Volunteer(VolunteerId id) : base(id)
    {
    }

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description generalDescription,
        Experience experience,
        PhoneNumber number,
        List<SocialLink> socialLinks,
        List<Requisite> requisites) : base(id)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        Experience = experience;
        PhoneNumber = number;
        _socialLinks.AddRange(socialLinks);

        _requisites.AddRange(requisites);
    }

    public FullName FullName { get; private set; }
    public Description GeneralDescription { get; private set; }
    public Experience Experience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets.AsReadOnly();
    public IReadOnlyList<SocialLink> SocialLinks => _socialLinks.AsReadOnly();
    public IReadOnlyList<Requisite> Requisites => _requisites.AsReadOnly();

    public void AddPet(Pet pet)
    {
        pet.SetPosition(Position.Create(_pets.Count + 1).Value);

        _pets.Add(pet);
    }

    public UnitResult<Error> RemovePet(Pet pet)
    {
        if (_pets.Contains(pet) == false)
            return Error.Validation("pet.not.found", "Pet not found");

        var petPosition = pet.Position;
        foreach (var otherPet in _pets.Where(p =>
                     p.Position.Value > petPosition.Value))
        {
            otherPet.SetPosition(Position.Create(otherPet.Position.Value - 1).Value);
        }

        _pets.Remove(pet);

        return Result.Success<Error>();
    }

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

    public void UpdateSocialLinks(List<SocialLink> list) =>
        _socialLinks = list;

    public void UpdateRequisites(List<Requisite> list) =>
        _requisites = list;

    public Pet? GetById(PetId id) => _pets.FirstOrDefault(x => x.Id == id);

    public UnitResult<Error> UpdatePet(
        PetId petId,
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
        List<Requisite>? requisites
    )
    {
        var pet = GetById(petId);
        if (pet == null)
            return Errors.General.NotFound(petId.Id);

        pet.UpdatePet(
            petName,
            generalDescription,
            healthInformation,
            animalType,
            address,
            attributes,
            birthDate,
            isCastrated,
            isVaccinated,
            helpStatus,
            requisites);

        return Result.Success<Error>();
    }

    public UnitResult<Error> ChangePetPosition(Pet pet, Position newPosition)
    {
        if (_pets.Contains(pet) == false)
            return Error.Validation("pet.not.found", "Pet not found");

        if (newPosition.Value > _pets.Count)
            return Error.Validation("position.is.invalid", "Position is greater than pets count");

        var currentPosition = pet.Position;

        if (currentPosition == newPosition)
            return Result.Success<Error>();

        // Перемещение вверх по позиции (уменьшение значения позиции)
        if (newPosition.Value < currentPosition.Value)
        {
            foreach (var otherPet in _pets.Where(p =>
                         p.Position.Value >= newPosition.Value &&
                         p.Position.Value < currentPosition.Value))
            {
                otherPet.SetPosition(Position.Create(otherPet.Position.Value + 1).Value);
            }
        }
        // Перемещение вниз по позиции (увеличение значения позиции)
        else
        {
            foreach (var otherPet in _pets.Where(p =>
                         p.Position.Value > currentPosition.Value
                         && p.Position.Value <= newPosition.Value))
            {
                otherPet.SetPosition(Position.Create(otherPet.Position.Value - 1).Value);
            }
        }

        // Установка новой позиции для перемещаемого питомца
        pet.SetPosition(newPosition);

        return Result.Success<Error>();
    }

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