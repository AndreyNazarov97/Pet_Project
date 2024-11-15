using System.Transactions;
using CSharpFunctionalExtensions;
using PetProject.Application.Abstractions;
using PetProject.Application.Dto;
using PetProject.Application.SpeciesManagement;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.SpeciesManagement.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Volunteers.CreatePet;

public class CreatePetHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePetHandler(
        IVolunteersRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreatePetCommand command,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var volunteer = volunteerResult.Value;
        var pet = await CreatePet(command, volunteer.PhoneNumber, cancellationToken);
        
        volunteer.AddPet(pet);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
 
        return pet.Id.Id;
    }

    private async Task<Pet> CreatePet(
        CreatePetCommand command,
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken)
    {
        var petId = PetId.NewId();
        var petName = PetName.Create(command.Name).Value;
        var generalDescription = Description.Create(command.GeneralDescription).Value;
        var healthInformation = Description.Create(command.HealthInformation).Value;

        var animalType = (await GetAnimalType(command, cancellationToken)).Value;

        var address = GetAddress(command.Address);

        var petPhysicalAttributes = PetPhysicalAttributes.Create(command.Weight, command.Height).Value;

        var pet = new Pet(
            petId,
            petName,
            generalDescription,
            healthInformation,
            animalType,
            address,
            petPhysicalAttributes,
            phoneNumber,
            command.BirthDate,
            command.IsCastrated,
            command.IsVaccinated,
            command.HelpStatus,
            new RequisitesList([Requisite.Create("test", "test").Value]),
            new PetPhotosList([])
        );

        return pet;
    }

    private async Task<Result<AnimalType, Error>> GetAnimalType(CreatePetCommand command,
        CancellationToken cancellationToken)
    {
        var speciesName = SpeciesName.Create(command.Species).Value;
        var speciesResult = await _speciesRepository.GetByName(speciesName, cancellationToken);
        if (speciesResult.IsFailure)
            return speciesResult.Error;

        var breedName = BreedName.Create(command.Breed).Value;
        var breed = speciesResult.Value.Breeds.FirstOrDefault(b => b.BreedName == breedName);
        if (breed is null)
            return Errors.General.NotFound();

        var animalType = new AnimalType(speciesResult.Value.Id, breed.Id);
        return animalType;
    }

    private Address GetAddress(AddressDto addressDto)
    {
        var address = Address.Create(addressDto.Country, addressDto.City, addressDto.Street, addressDto.House,
            addressDto.Flat).Value;

        return address;
    }
}