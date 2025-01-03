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

public class Volunteer : AggregateRoot<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    private Volunteer(VolunteerId id) : base(id)
    {
    }

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description generalDescription,
        Experience experience,
        PhoneNumber number) : base(id)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        Experience = experience;
        PhoneNumber = number;
    }

    public FullName FullName { get; private set; }
    public Description GeneralDescription { get; private set; }
    public Experience Experience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets.AsReadOnly();

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

    public Pet? GetById(PetId id) => _pets.FirstOrDefault(x => x.Id == id);

    public UnitResult<Error> AddPetPhoto(PetId petId, IEnumerable<PetPhoto> petPhotos)
    {
        var pet = GetById(petId);
        if (pet == null)
            return Errors.General.NotFound(petId.Id);

        pet.AddPhotos(petPhotos);
        return Result.Success<Error>();
    }

    public UnitResult<Error> DeletePetPhoto(PetId petId, FilePath filePath)
    {
        var pet = GetById(petId);
        if (pet == null)
            return Errors.General.NotFound(petId.Id);

        var result = pet.DeletePhoto(filePath);
        if (result.IsFailure)
            return result.Error;

        return Result.Success<Error>();
    }

    public UnitResult<Error> SetPetMainPhoto(PetId petId, PetPhoto petPhoto)
    {
        var pet = GetById(petId);
        if (pet == null)
            return Errors.General.NotFound(petId.Id);

        var result = pet.SetMainPhoto(petPhoto);
        if (result.IsFailure)
            return result.Error;

        return Result.Success<Error>();
    }

    public UnitResult<Error> ChangePetStatus(PetId petId, HelpStatus helpStatus)
    {
        var pet = GetById(petId);
        if (pet == null)
            return Errors.General.NotFound(petId.Id);

        pet.ChangeStatus(helpStatus);
        return Result.Success<Error>();
    }

    public UnitResult<Error> UpdatePet(
        PetId petId,
        PetName? petName,
        Description? generalDescription,
        Description? healthInformation,
        AnimalType? animalType,
        Address? address,
        PetPhysicalAttributes? attributes,
        DateTimeOffset? birthDate,
        bool? isCastrated,
        bool? isVaccinated,
        HelpStatus? helpStatus
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
            helpStatus);

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

    public UnitResult<Error> DeletePetSoft(PetId petId, DateTimeOffset deletionDate)
    {
        var pet = GetById(petId);
        if (pet == null)
            return Errors.General.NotFound(petId.Id);
        
        RecalculatePetsPosition(pet.Position);
        pet.Delete(deletionDate);
        
        return Result.Success<Error>();
    }
    public UnitResult<Error> DeletePetHard(PetId petId, DateTimeOffset deletionDate)
    {
        var pet = GetById(petId);
        if (pet == null)
            return Errors.General.NotFound(petId.Id);
        
        RecalculatePetsPosition(pet.Position);
        _pets.Remove(pet);
        pet.Delete(deletionDate);
        
        return Result.Success<Error>();
    }

    public override void Delete(DateTimeOffset deletionDate)
    {
        foreach (var pet in _pets)
        {
            pet.Delete(deletionDate);
        }
        
        base.Delete(deletionDate);
    }

    private void RecalculatePetsPosition(Position petPosition)
    {
        if(petPosition.Value == _pets.Count)
            return;

        var petsToMove = _pets.Where(p => p.Position.Value > petPosition.Value);

        foreach (var pet in petsToMove)
        {
            pet.SetPosition(Position.Create(pet.Position.Value - 1).Value);
        }
    }
}