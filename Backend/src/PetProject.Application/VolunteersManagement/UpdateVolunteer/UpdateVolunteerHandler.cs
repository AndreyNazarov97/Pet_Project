using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Application.Abstractions;
using PetProject.Application.Models;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.VolunteersManagement.UpdateVolunteer;

public class UpdateVolunteerHandler : IRequestHandler<UpdateVolunteerCommand, Result<Guid, ErrorList>>
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

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerCommand command, 
        CancellationToken cancellationToken = default)
    {
        var volunteerQuery = new VolunteerQueryModel()
        {
            VolunteerIds = [command.VolunteerId]
        };

        var volunteer = (await _repository.Query(volunteerQuery, cancellationToken)).SingleOrDefault();
        if (volunteer == null)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();
        
        var volunteerEntity = volunteer.ToEntity();

        var fullName = FullName.Create(
                command.FullName.Name, 
                command.FullName.Surname, 
                command.FullName.Patronymic)
            .Value;
        var description = Description.Create(command.Description).Value;
        var ageExperience = Experience.Create(command.AgeExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        volunteerEntity.UpdateMainInfo(fullName, description, ageExperience, phoneNumber);
        //TODO: здесь сущность не отслеживается, соответсвенно не обновляется. То же и в схожих методах

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogDebug("Volunteer {volunteerId} was full updated", command.VolunteerId);

        return volunteerEntity.Id.Id;
    }
}