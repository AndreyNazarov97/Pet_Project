using PetProject.Application.UseCases.GetVolunteer;
using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using Serilog;

namespace PetProject.Application.UseCases.UpdateVolunteer;

public class UpdateVolunteerUseCase : IUpdateVolunteerUseCase
{
    private readonly IGetVolunteerStorage _getVolunteerStorage;
    private readonly ILogger _logger;

    public UpdateVolunteerUseCase(
        IGetVolunteerStorage getVolunteerStorage,
        ILogger logger)
    {
        _getVolunteerStorage = getVolunteerStorage;
        _logger = logger;
    }

    public async Task<Result> UpdateMainInfo(VolunteerId id, UpdateMainInfoRequest request,
        CancellationToken cancellationToken)
    {
        var volunteer = await _getVolunteerStorage.GetVolunteer(id, cancellationToken);
        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound(id);
        }
        
        var fullName = FullName.Create(request.FirstName, request.LastName, request.Patronymic).Value;
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;
        var description = NotNullableText.Create(request.Description).Value; 
        var experience = Experience.Create(request.Experience).Value;
        
        volunteer.Value.UpdateMainInfo(fullName, phoneNumber, description, experience);

        await _getVolunteerStorage.SaveChangesAsync(cancellationToken);
        
        _logger.Information("Volunteer {id} was updated", id);
        return Result.Success();
    }

    public Task<Result> UpdateSocialNetworks(VolunteerId id, UpdateSocialNetworksRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateRequisites(VolunteerId id, UpdateRequisitesRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}