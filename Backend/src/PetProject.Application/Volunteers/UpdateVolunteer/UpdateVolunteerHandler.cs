using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Volunteers.UpdateVolunteer;

public class UpdateVolunteerHandler(IVolunteersRepository repository, ILogger<UpdateVolunteerHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(UpdateVolunteerCommand command, CancellationToken token = default)
    {
        var volunteerId = VolunteerId.Create(command.IdVolunteer);

        var volunteer = await repository.GetById(volunteerId, token);

        if (volunteer.IsFailure)
            return volunteer.Error;

        var fullName = FullName.Create(
                command.Dto.FullName.Name, 
                command.Dto.FullName.Surname, 
                command.Dto.FullName.Patronymic)
            .Value;
        var description = Description.Create(command.Dto.Description).Value;
        var ageExperience = Experience.Create(command.Dto.AgeExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.Dto.PhoneNumber).Value;

        volunteer.Value.UpdateMainInfo(fullName, description, ageExperience, phoneNumber);

        var resultUpdate = await repository.Save(volunteer.Value, token);
        if (resultUpdate.IsFailure)
            return resultUpdate.Error;

        logger.LogDebug("Volunteer {volunteerId} was full updated", volunteerId);

        return resultUpdate.Value;
    }
}