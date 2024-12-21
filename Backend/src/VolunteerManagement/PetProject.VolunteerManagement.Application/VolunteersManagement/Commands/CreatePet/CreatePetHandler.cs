using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Database;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Domain.Aggregate;
using PetProject.VolunteerManagement.Domain.Entities;
using PetProject.VolunteerManagement.Domain.ValueObjects;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.CreatePet;

public class CreatePetHandler : IRequestHandler<CreatePetCommand, Result<Guid, ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IReadRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePetHandler(
        IVolunteersRepository volunteersRepository,
        IReadRepository readRepository,
        [FromKeyedServices(Constants.Context.VolunteerManagement)]IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _readRepository = readRepository;
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
        var species = (await _readRepository.QuerySpecies(speciesQuery, cancellationToken)).SingleOrDefault();
        if (species == null)
            return Errors.General.NotFound();
        
        var breed = species.Breeds.FirstOrDefault(b => b.BreedName == command.BreedName);
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