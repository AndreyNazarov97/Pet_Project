using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.UpdateVolunteer;

public class UpdateVolunteerHandler : IRequestHandler<UpdateVolunteerCommand, Result<Guid, ErrorList>>
{
    private readonly IVolunteersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdateVolunteerHandler(
        IVolunteersRepository repository,
        [FromKeyedServices(Constants.Context.VolunteerManagement)] IUnitOfWork unitOfWork,
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
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteer = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

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