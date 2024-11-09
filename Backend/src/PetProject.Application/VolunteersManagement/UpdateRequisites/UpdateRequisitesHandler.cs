using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.VolunteersManagement.UpdateRequisites;

public class UpdateRequisitesHandler(IVolunteersRepository repository, ILogger<UpdateRequisitesHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(UpdateRequisitesRequest request,
        CancellationToken token = default)
    {
        var volunteerId = VolunteerId.Create(request.Id);

        var volunteer = await repository.GetById(volunteerId, token);

        if (volunteer.IsFailure)
            return volunteer.Error;

        var requisites = request.Dto.Requisites
            .Select(x => Requisite.Create(x.Name, x.Description).Value);
        var requisitesList = new RequisitesList(requisites);

        volunteer.Value.UpdateRequisites(requisitesList);
        
        var resultUpdate = await repository.Save(volunteer.Value, token);
        if (resultUpdate.IsFailure)
            return resultUpdate.Error;

        logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated requisites", volunteerId);
        return resultUpdate.Value;
    }
}