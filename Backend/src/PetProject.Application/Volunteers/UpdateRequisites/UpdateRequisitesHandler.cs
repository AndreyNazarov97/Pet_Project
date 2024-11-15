using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Volunteers.UpdateRequisites;

public class UpdateRequisitesHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateRequisitesHandler> _logger;

    public UpdateRequisitesHandler(
        IVolunteersRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateRequisitesHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Execute(UpdateRequisitesCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.Id);

        var volunteer = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteer.IsFailure)
            return volunteer.Error;

        var requisites = command.Dto.Requisites
            .Select(x => Requisite.Create(x.Name, x.Description).Value);
        var requisitesList = new RequisitesList(requisites);

        volunteer.Value.UpdateRequisites(requisitesList);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated requisites", volunteerId);
        return volunteer.Value.Id.Id;
    }
}