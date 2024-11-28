using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Abstractions;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Application.SpeciesManagement;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.SpeciesManagement.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.VolunteersManagement.CreatePet;

public class CreatePetHandler : IRequestHandler<CreatePetCommand, Result<Guid, ErrorList>>
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

    public async Task<Result<Guid, ErrorList>> Handle(
        CreatePetCommand command,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var volunteer = volunteerResult.Value;
        var petResult = await CreatePet(command, volunteer, cancellationToken);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();
        
        var pet = petResult.Value;
        
        volunteer.AddPet(pet);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
 
        return pet.Id.Id;
    }

    private async Task<Result<Pet, Error>> CreatePet(
        CreatePetCommand command,
        Volunteer volunteer,
        CancellationToken cancellationToken)
    {
        var petId = PetId.NewId();
        var petName = PetName.Create(command.Name).Value;
        var generalDescription = Description.Create(command.GeneralDescription).Value;
        var healthInformation = Description.Create(command.HealthInformation).Value;

        var animalTypeResult = (await GetAnimalType(command, cancellationToken));
        if (animalTypeResult.IsFailure)
            return animalTypeResult.Error;
        
        var animalType = animalTypeResult.Value;

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
            volunteer.PhoneNumber,
            command.BirthDate,
            command.IsCastrated,
            command.IsVaccinated,
            command.HelpStatus,
            volunteer.Requisites.ToList(),
            []
        );

        return pet;
    }

    private async Task<Result<AnimalType, Error>> GetAnimalType(CreatePetCommand command,
        CancellationToken cancellationToken)
    {
        var speciesQuery = new SpeciesQueryModel
        {
            SpeciesName = command.SpeciesName
        };
        var species = (await _speciesRepository.Query(speciesQuery, cancellationToken)).SingleOrDefault();
        if (species == null)
            return Errors.General.NotFound();
        
        var breed = species.Breeds.FirstOrDefault(b => b.Name == command.BreedName);
        if (breed is null)
            return Errors.General.NotFound();
        
        var speciesName = SpeciesName.Create(command.SpeciesName).Value;
        var breedName = BreedName.Create(command.BreedName).Value;

        var animalType = new AnimalType(speciesName, breedName);
        return animalType;
    }

    private Address GetAddress(AddressDto addressDto)
    {
        var address = Address.Create(addressDto.Country, addressDto.City, addressDto.Street, addressDto.House,
            addressDto.Flat).Value;

        return address;
    }
}