using PetProject.Application.UseCases.Volunteer.GetVolunteer;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using Serilog;

namespace PetProject.Application.UseCases.Volunteer.UpdateRequisites;

public class UpdateRequisitesUseCase : IUpdateRequisitesUseCase
{
    private readonly IGetVolunteerStorage _getVolunteerStorage;
    private readonly ILogger _logger;

    public UpdateRequisitesUseCase(
        IGetVolunteerStorage getVolunteerStorage,
        ILogger logger)
    {
        _getVolunteerStorage = getVolunteerStorage;
        _logger = logger;
    }

    public async Task<Result> UpdateRequisites(UpdateRequisitesRequest request, CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.FromGuid(request.VolunteerId);
        var volunteer = await _getVolunteerStorage.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound(volunteerId);
        }

        var requisites = request.Dto.Requisites
            .Select(r => Requisite.Create(r.Title, r.Description).Value).ToList();
        var requisitesList = new RequisitesList(requisites);

        volunteer.Value.UpdateRequisites(requisitesList);
        await _getVolunteerStorage.SaveChangesAsync(cancellationToken);

        _logger.Information("Volunteer {id} requisites was updated", volunteerId);
        return Result.Success();
    }
}