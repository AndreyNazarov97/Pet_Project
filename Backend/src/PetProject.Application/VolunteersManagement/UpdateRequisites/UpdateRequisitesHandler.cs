using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.VolunteersManagement.UpdateRequisites;

public class UpdateRequisitesHandler : IRequestHandler<UpdateRequisitesCommand, Result<Guid, ErrorList>>
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

    public async Task<Result<Guid, ErrorList>> Handle(UpdateRequisitesCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteer = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var requisites = command.Requisites
            .Select(x => Requisite.Create(x.Title, x.Description).Value);
        var requisitesList = new RequisitesList(requisites);

        volunteer.Value.UpdateRequisites(requisitesList);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated requisites", volunteerId);
        return volunteer.Value.Id.Id;
    }
}