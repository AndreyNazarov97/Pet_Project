using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.Volunteers.DeleteVolunteer;

public class DeleteVolunteerHandler(IVolunteersRepository repository, ILogger<DeleteVolunteerHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(DeleteVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(request.Id);
        var volunteerResult = await repository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        volunteerResult.Value.Deactivate();
        
        var result = await repository.Delete(volunteerResult.Value, cancellationToken);
        if (result.IsFailure)
            return volunteerResult.Error;
        
        logger.Log(LogLevel.Information, "Volunteer deleted with Id {volunteerId}", volunteerId);
        
        return volunteerResult.Value.Id.Id;
    }
}