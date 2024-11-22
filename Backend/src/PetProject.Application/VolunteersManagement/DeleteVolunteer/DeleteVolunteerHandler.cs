using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Application.Models;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.DeleteVolunteer;

public class DeleteVolunteerHandler : IRequestHandler<DeleteVolunteerCommand, UnitResult<ErrorList>>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(IVolunteersRepository repository, ILogger<DeleteVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerQuery = new VolunteerQueryModel()
        {
            VolunteerIds = [command.Id]
        };

        var volunteer = (await _repository.Query(volunteerQuery, cancellationToken)).SingleOrDefault();
        if(volunteer == null) 
            return Errors.General.NotFound().ToErrorList();

        var volunteerEntity = volunteer.ToEntity();
        volunteerEntity.Deactivate();
        
        var result = await _repository.Delete(volunteerEntity, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToErrorList();
        
        _logger.Log(LogLevel.Information, "Volunteer deleted with Id {volunteerId}", command.Id);
        
        return UnitResult.Success<ErrorList>();
    }
}