using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Application.Models;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.VolunteersManagement.CreateVolunteer;

public class CreateVolunteerHandler : IRequestHandler<CreateVolunteerCommand, Result<Guid, ErrorList>>
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

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command, CancellationToken token = default
    )
    {
        var volunteerQuery = new VolunteerQueryModel()
        {
            PhoneNumber = command.PhoneNumber
        };

        var existedVolunteer = await _repository.Query(volunteerQuery, token);
        if (existedVolunteer.Length > 0)
        {
            return Errors.General.AlreadyExist("Volunteer").ToErrorList();
        }
        
        var volunteerId = VolunteerId.NewId();

        var fullName = FullName.Create(
            command.FullName.Name, 
            command.FullName.Surname, 
            command.FullName.Patronymic).Value;
        var description = Description.Create(command.Description).Value;
        var ageExperience = Experience.Create(command.AgeExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var socialLinks = command.SocialLinks
            .Select(x => SocialLink.Create(x.Title, x.Url).Value)
            .ToList();

        var requisites = command.Requisites
            .Select(x => Requisite.Create(x.Title, x.Description).Value)
            .ToList();

        var volunteer = new Volunteer(
            volunteerId,
            fullName, 
            description,
            ageExperience, 
            phoneNumber, 
            socialLinks, 
            requisites);

        await _repository.Add(volunteer, token);
        
        _logger.Log(LogLevel.Information, "Created new volunteer: {VolunteerId}", volunteerId);
        
        return volunteerId.Id;
    }
}