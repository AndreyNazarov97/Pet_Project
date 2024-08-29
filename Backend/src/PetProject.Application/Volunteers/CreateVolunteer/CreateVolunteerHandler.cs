using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler(IVolunteersRepository repository, ILogger<CreateVolunteerHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(
        CreateVolunteerRequest request, CancellationToken token = default
    )
    {
        var phoneNumber = PhoneNumber.Create(request.Number);

        var existedVolunteer = await repository.GetByPhoneNumber(phoneNumber.Value, token);

        if (existedVolunteer.IsSuccess)
            return Errors.Model.AlreadyExist("Volunteer");

        var volunteerId = VolunteerId.NewId();

        var fullName = FullName.Create(request.FullName.Name, request.FullName.Surname, request.FullName.Patronymic);
        var description = Description.Create(request.Description);
        var ageExperience = AgeExperience.Create(request.AgeExperience);

        var socialLinks = request.SocialLinks
            .Select(x => SocialLink.Create(x.Name, x.Url).Value);
        var socialLinksList = new SocialLinksList(socialLinks);

        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.Name, x.Description).Value);
        var requisitesList = new RequisitesList(requisites);

        var volunteer = new Volunteer(volunteerId,
            fullName.Value, description.Value,
            ageExperience.Value, phoneNumber.Value, socialLinksList, requisitesList);

        await repository.Add(volunteer, token);
        
        logger.Log(LogLevel.Information, "Created new volunteer: {VolunteerId}", volunteerId);
        
        return volunteerId.Id;
    }
}