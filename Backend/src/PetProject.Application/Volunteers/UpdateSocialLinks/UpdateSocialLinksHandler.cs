using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Volunteers.UpdateSocialLinks;

public class UpdateSocialLinksHandler(IVolunteersRepository repository, ILogger<UpdateSocialLinksHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(UpdateSocialLinksRequest request,
        CancellationToken token = default)
    {
        var volunteerId = VolunteerId.Create(request.Id);

        var volunteer = await repository.GetById(volunteerId, token);

        if (volunteer.IsFailure)
            return volunteer.Error;

        var socialLinks = request.Dto.SocialLinks
            .Select(x => SocialLink.Create(x.Name, x.Url).Value);
        var socialLinksList = new SocialLinksList(socialLinks);

        volunteer.Value.UpdateSocialLinks(socialLinksList);
        var resultUpdate = await repository.Save(volunteer.Value, token);
        if (resultUpdate.IsFailure)
            return resultUpdate.Error;

        logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated social links", volunteerId);

        return resultUpdate;
    }
}