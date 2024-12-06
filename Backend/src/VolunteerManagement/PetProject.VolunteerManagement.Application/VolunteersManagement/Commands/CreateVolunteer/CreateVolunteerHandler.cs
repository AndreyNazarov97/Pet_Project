using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Domain.Aggregate;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.CreateVolunteer;

public class CreateVolunteerHandler : IRequestHandler<CreateVolunteerCommand, Result<Guid, ErrorList>>
{
    private readonly IVolunteersRepository _repository;
    private readonly IReadRepository _readRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository repository,
        IReadRepository readRepository,
        ILogger<CreateVolunteerHandler> logger)
    {
        _repository = repository;
        _readRepository = readRepository;
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

        var existedVolunteer = await _readRepository.QueryVolunteers(volunteerQuery, token);
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

        var volunteer = new Volunteer(
            volunteerId,
            fullName, 
            description,
            ageExperience, 
            phoneNumber);

        await _repository.Add(volunteer, token);
        
        _logger.Log(LogLevel.Information, "Created new volunteer: {VolunteerId}", volunteerId);
        
        return volunteerId.Id;
    }
}