using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.VolunteersManagement.CreateVolunteer;

public class CreateVolunteerHandler : IRequestHandler<CreateVolunteerCommand, Result<Guid, Error>>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository repository,
        ILogger<CreateVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerCommand command, CancellationToken token = default
    )
    {
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber);

        var existedVolunteer = await _repository.GetByPhoneNumber(phoneNumber.Value, token);

        if (existedVolunteer.IsSuccess)
            return Errors.Model.AlreadyExist("Volunteer");

        var volunteerId = VolunteerId.NewId();

        var fullName = FullName.Create(command.FullName.Name, command.FullName.Surname, command.FullName.Patronymic);
        var description = Description.Create(command.Description);
        var ageExperience = Experience.Create(command.AgeExperience);

        var socialLinks = command.SocialLinks
            .Select(x => SocialLink.Create(x.Name, x.Url).Value);
        var socialLinksList = new SocialLinksList(socialLinks);

        var requisites = command.Requisites
            .Select(x => Requisite.Create(x.Name, x.Description).Value);
        var requisitesList = new RequisitesList(requisites);

        var volunteer = new Volunteer(volunteerId,
            fullName.Value, description.Value,
            ageExperience.Value, phoneNumber.Value, socialLinksList, requisitesList);

        await _repository.Add(volunteer, token);
        
        _logger.Log(LogLevel.Information, "Created new volunteer: {VolunteerId}", volunteerId);
        
        return volunteerId.Id;
    }
}