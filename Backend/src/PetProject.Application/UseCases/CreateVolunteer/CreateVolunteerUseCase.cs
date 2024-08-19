﻿using FluentValidation;
using PetProject.Application.UseCases.GetVolunteer;
using PetProject.Domain.Entities;
using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Shared;
using Serilog;

namespace PetProject.Application.UseCases.CreateVolunteer;

public class CreateVolunteerUseCase : ICreateVolunteerUseCase
{
    private readonly ICreateVolunteerStorage _storage;
    private readonly IGetVolunteerStorage _getVolunteerStorage;
    private readonly IValidator<CreateVolunteerRequest> _createVolunteerRequestValidator;
    private readonly ILogger _logger;

    public CreateVolunteerUseCase(
        ICreateVolunteerStorage storage,
        IValidator<CreateVolunteerRequest> createVolunteerRequestValidator,
        ILogger logger)
        IGetVolunteerStorage getVolunteerStorage,
        IValidator<CreateVolunteerRequest> createVolunteerRequestValidator)
    {
        _storage = storage;
        _getVolunteerStorage = getVolunteerStorage;
        _createVolunteerRequestValidator = createVolunteerRequestValidator;
        _logger = logger;
    }

    public async Task<Result<VolunteerId>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken)
    {
        var validation = await _createVolunteerRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return Errors.General.ValueIsInvalid("request");
        }

        var socialNetworks = request.SocialNetworks.Select(s =>
            SocialNetwork.Create(s.Title, s.Link).Value).ToList();
        var requisites = request.Requisites.Select(r =>
            Requisite.Create(r.Title, r.Description).Value).ToList();
        var details = VolunteerDetails.Create(
            requisites, 
            socialNetworks);
        if(details.IsFailure)
        {
            return Result<VolunteerId>.Failure(details.Error!);
        }
        
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        if (phoneNumber.IsFailure)
        {
            return Result<VolunteerId>.Failure(phoneNumber.Error!);
        }
        
        var fullName = FullName.Create(request.FirstName, request.LastName, request.Patronymic);
        if (fullName.IsFailure)
        {
            return Result<VolunteerId>.Failure(fullName.Error!);
        }
        
        var existedPhoneNumber = await _getVolunteerStorage.GetByPhone(phoneNumber.Value, cancellationToken);
        if (existedPhoneNumber.IsSuccess)
        {
            return Errors.Volunteer.PhoneNumberAlreadyExists();
        }

        var volunteerEntity = Volunteer.Create(
            VolunteerId.NewVolunteerId(),
            fullName.Value,
            request.Description,
            request.Experience,
            phoneNumber.Value,
            details.Value,
            null);

        _logger.Information("Create volunteer: {Volunteer}", volunteerEntity.Value);

        if (volunteerEntity.IsFailure)
        {
            return Result<VolunteerId>.Failure(volunteerEntity.Error!);
        }

        return await _storage.CreateVolunteer(volunteerEntity.Value, cancellationToken);
    }
}