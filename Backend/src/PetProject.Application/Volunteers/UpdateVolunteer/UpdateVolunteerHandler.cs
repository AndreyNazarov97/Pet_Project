using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Volunteers.UpdateVolunteer;

public class UpdateVolunteerHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdateVolunteerHandler(
        IVolunteersRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Execute(
        UpdateVolunteerCommand command, 
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.IdVolunteer);

        var volunteer = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteer.IsFailure)
            return volunteer.Error;

        var fullName = FullName.Create(
                command.Dto.FullName.Name, 
                command.Dto.FullName.Surname, 
                command.Dto.FullName.Patronymic)
            .Value;
        var description = Description.Create(command.Dto.Description).Value;
        var ageExperience = Experience.Create(command.Dto.AgeExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.Dto.PhoneNumber).Value;

        volunteer.Value.UpdateMainInfo(fullName, description, ageExperience, phoneNumber);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogDebug("Volunteer {volunteerId} was full updated", volunteerId);

        return volunteer.Value.Id.Id;
    }
}