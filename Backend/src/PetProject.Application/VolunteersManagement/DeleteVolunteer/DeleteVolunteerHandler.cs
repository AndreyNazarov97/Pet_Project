using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.VolunteersManagement.DeleteVolunteer;

public class DeleteVolunteerHandler : IRequestHandler<DeleteVolunteerCommand, Result<Guid, Error>>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(IVolunteersRepository repository, ILogger<DeleteVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.Id);
        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        volunteerResult.Value.Deactivate();
        
        var result = await _repository.Delete(volunteerResult.Value, cancellationToken);
        if (result.IsFailure)
            return volunteerResult.Error;
        
        _logger.Log(LogLevel.Information, "Volunteer deleted with Id {volunteerId}", volunteerId);
        
        return volunteerResult.Value.Id.Id;
    }
}