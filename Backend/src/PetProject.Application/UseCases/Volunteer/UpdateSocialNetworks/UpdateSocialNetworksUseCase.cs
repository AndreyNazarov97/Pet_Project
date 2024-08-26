using PetProject.Application.UseCases.Volunteer.GetVolunteer;
using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using Serilog;

namespace PetProject.Application.UseCases.Volunteer.UpdateSocialNetworks;

public class UpdateSocialNetworksUseCase : IUpdateSocialNetworksUseCase
{
    private readonly IGetVolunteerStorage _getVolunteerStorage;
    private readonly ILogger _logger;

    public UpdateSocialNetworksUseCase(
        IGetVolunteerStorage getVolunteerStorage,
        ILogger logger)
    {
        _getVolunteerStorage = getVolunteerStorage;
        _logger = logger;
    }
    public async Task<Result> UpdateSocialNetworks(UpdateSocialNetworksRequest request, CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.FromGuid(request.VolunteerId);
        var volunteer = await _getVolunteerStorage.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
        {
            return Errors.General.NotFound(volunteerId);
        }

        var socialNetworks = request.Dto.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Title, s.Link).Value).ToList();
       
        volunteer.Value.UpdateSocialNetworks(socialNetworks);
        await _getVolunteerStorage.SaveChangesAsync(cancellationToken);
        
        _logger.Information("Volunteer {id} social networks was updated", volunteerId);
        return Result.Success();
    }
}