using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.VolunteersManagement.UpdateVolunteer;

public class UpdateVolunteerHandler : IRequestHandler<UpdateVolunteerCommand, Result<Guid, Error>>
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

    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerCommand command, 
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.IdVolunteer);

        var volunteer = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteer.IsFailure)
            return volunteer.Error;

        var fullName = FullName.Create(
                command.FullName.Name, 
                command.FullName.Surname, 
                command.FullName.Patronymic)
            .Value;
        var description = Description.Create(command.Description).Value;
        var ageExperience = Experience.Create(command.AgeExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        volunteer.Value.UpdateMainInfo(fullName, description, ageExperience, phoneNumber);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogDebug("Volunteer {volunteerId} was full updated", volunteerId);

        return volunteer.Value.Id.Id;
    }
}