using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.VolunteersManagement.UpdateVolunteer;

public class UpdateVolunteerHandler(IVolunteersRepository repository, ILogger<UpdateVolunteerHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(UpdateVolunteerRequest request, CancellationToken token = default)
    {
        var volunteerId = VolunteerId.Create(request.IdVolunteer);

        var volunteer = await repository.GetById(volunteerId, token);

        if (volunteer.IsFailure)
            return volunteer.Error;

        var fullName = FullName.Create(
                request.Dto.FullName.Name, 
                request.Dto.FullName.Surname, 
                request.Dto.FullName.Patronymic)
            .Value;
        var description = Description.Create(request.Dto.Description).Value;
        var ageExperience = Experience.Create(request.Dto.AgeExperience).Value;
        var phoneNumber = PhoneNumber.Create(request.Dto.PhoneNumber).Value;

        volunteer.Value.UpdateMainInfo(fullName, description, ageExperience, phoneNumber);

        var resultUpdate = await repository.Save(volunteer.Value, token);
        if (resultUpdate.IsFailure)
            return resultUpdate.Error;

        logger.LogDebug("Volunteer {volunteerId} was full updated", volunteerId);

        return resultUpdate.Value;
    }
}