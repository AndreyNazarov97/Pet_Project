using PetProject.Application.UseCases.Volunteer.GetVolunteer;
using PetProject.Application.UseCases.Volunteer.UpdateRequisites;
using PetProject.Application.UseCases.Volunteer.UpdateSocialNetworks;
using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using Serilog;

namespace PetProject.Application.UseCases.Volunteer.UpdateMainInfo;

public class UpdateMainInfoUseCase : IUpdateMainInfoUseCase
{
    private readonly IGetVolunteerStorage _getVolunteerStorage;
    private readonly ILogger _logger;

    public UpdateMainInfoUseCase(
        IGetVolunteerStorage getVolunteerStorage,
        ILogger logger)
    {
        _getVolunteerStorage = getVolunteerStorage;
        _logger = logger;
    }

    public async Task<Result> UpdateMainInfo(
        UpdateMainInfoRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.FromGuid(request.VolunteerId);
        var volunteer = await _getVolunteerStorage.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound(volunteerId);
        }
        
        var fullName = FullName.Create(request.Dto.FirstName, request.Dto.LastName, request.Dto.Patronymic).Value;
        var phoneNumber = PhoneNumber.Create(request.Dto.PhoneNumber).Value;
        var description = NotNullableText.Create(request.Dto.Description).Value; 
        var experience = Experience.Create(request.Dto.Experience).Value;
        
        volunteer.Value.UpdateMainInfo(fullName, phoneNumber, description, experience);

        await _getVolunteerStorage.SaveChangesAsync(cancellationToken);
        
        _logger.Information("Volunteer {id} was updated", volunteerId);
        return Result.Success();
    }

    public Task<Result> UpdateSocialNetworks(
        UpdateSocialNetworksRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateRequisites(
        UpdateRequisitesRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}