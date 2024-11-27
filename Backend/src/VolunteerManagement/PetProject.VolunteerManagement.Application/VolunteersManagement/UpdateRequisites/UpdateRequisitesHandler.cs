using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Domain.ValueObjects;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.UpdateRequisites;

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
            .Select(x => Requisite.Create(x.Title, x.Description).Value)
            .ToList();

        volunteer.Value.UpdateRequisites(requisites);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated requisites", volunteerId);
        return volunteer.Value.Id.Id;
    }
}