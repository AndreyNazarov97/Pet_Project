using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Database;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Domain.Aggregate;
using PetProject.VolunteerManagement.Domain.Enums;
using PetProject.VolunteerManagement.Domain.ValueObjects;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.UpdatePet;

public class UpdatePetHandler : IRequestHandler<UpdatePetCommand, Result<Guid, ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IReadRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePetHandler(
        IVolunteersRepository volunteersRepository,
        IReadRepository readRepository,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _readRepository = readRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(UpdatePetCommand command, CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var speciesQuery = new SpeciesQueryModel()
        {
            SpeciesName = command.PetInfo.SpeciesName ?? string.Empty,
            BreedName = command.PetInfo.BreedName ?? string.Empty
        };
        var speciesResult = await _readRepository.Query(speciesQuery, cancellationToken);
        if (speciesResult.Length == 0)
        {
            return Errors.General.NotFound().ToErrorList();
        }
        
        var volunteer = volunteerResult.Value;
        var updatePetResult = UpdatePet(volunteer, command);
        if (updatePetResult.IsFailure)
            return updatePetResult.Error.ToErrorList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return command.PetId;
    }

    private  UnitResult<Error> UpdatePet(Volunteer volunteer ,UpdatePetCommand command)
    {
        var petId = PetId.Create(command.PetId);
        
        var petName = command.PetInfo.PetName is null
            ? null
            : PetName.Create(command.PetInfo.PetName).Value;

        var generalDescription = command.PetInfo.GeneralDescription is null
            ? null
            : Description.Create(command.PetInfo.GeneralDescription).Value;
        
        var healthInformation = command.PetInfo.HealthInformation is null
            ? null
            : Description.Create(command.PetInfo.HealthInformation).Value;
        
        var animalType = command.PetInfo.SpeciesName is null || command.PetInfo.BreedName is null
            ? null
            : new AnimalType(SpeciesName.Create(command.PetInfo.SpeciesName).Value, BreedName.Create(command.PetInfo.BreedName).Value);
        
        var address = command.PetInfo.Address is null
            ? null
            : Address.Create(
                command.PetInfo.Address.Country,
                command.PetInfo.Address.City,
                command.PetInfo.Address.Street,
                command.PetInfo.Address.House,
                command.PetInfo.Address.Flat).Value;
        
        var petPhysicalAttributes = command.PetInfo.Weight is null || command.PetInfo.Height is null
            ? null
            : PetPhysicalAttributes.Create(command.PetInfo.Weight.Value, command.PetInfo.Height.Value).Value;
        
        var birthDate = command.PetInfo.BirthDate is null
            ? (DateOnly?)null
            : DateOnly.FromDateTime(command.PetInfo.BirthDate.Value);

        var result = volunteer.UpdatePet(
            petId,
            petName,
            generalDescription,
            healthInformation,
            animalType,
            address,
            petPhysicalAttributes,
            birthDate,
            command.PetInfo.IsCastrated,
            command.PetInfo.IsVaccinated,
            (HelpStatus?)command.PetInfo.HelpStatus,
            null);
        
        return result;
    }
}