using FluentValidation;
using PetProject.Application.UseCases.Volunteer.GetVolunteer;
using PetProject.Domain.PetManagement.Entities.Details;
using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using Serilog;

namespace PetProject.Application.UseCases.Volunteer.CreateVolunteer;

public class CreateVolunteerUseCase : ICreateVolunteerUseCase
{
    private readonly ICreateVolunteerStorage _storage;
    private readonly IGetVolunteerStorage _getVolunteerStorage;
    private readonly IValidator<CreateVolunteerRequest> _createVolunteerRequestValidator;
    private readonly ILogger _logger;

    public CreateVolunteerUseCase(
        ICreateVolunteerStorage storage,
        IGetVolunteerStorage getVolunteerStorage,
        ILogger logger,
        IValidator<CreateVolunteerRequest> createVolunteerRequestValidator)
    {
        _storage = storage;
        _getVolunteerStorage = getVolunteerStorage;
        _createVolunteerRequestValidator = createVolunteerRequestValidator;
        _logger = logger;
    }

    public async Task<Result<VolunteerId>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken)
    {
        var socialNetworks = request.SocialNetworks.Select(s =>
            SocialNetwork.Create(s.Title, s.Link).Value).ToList();
        var requisites = request.Requisites.Select(r =>
            Requisite.Create(r.Title, r.Description).Value).ToList();
        var details = new VolunteerDetails(requisites, socialNetworks);

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;
        var fullName = FullName.Create(request.FirstName, request.LastName, request.Patronymic).Value;
        var description = NotNullableText.Create(request.Description).Value;
        var experience = Experience.Create(request.Experience).Value;


        var existedPhoneNumber = await _getVolunteerStorage.GetByPhone(phoneNumber, cancellationToken);
        if (existedPhoneNumber.IsSuccess)
        {
            return Errors.Volunteer.PhoneNumberAlreadyExists();
        }


        var volunteer = new Domain.PetManagement.AggregateRoot.Volunteer(
            VolunteerId.NewVolunteerId(),
            fullName,
            description,
            experience,
            phoneNumber,
            details);

        _logger.Information("Create volunteer: {Volunteer}", volunteer);

        return await _storage.CreateVolunteer(volunteer, cancellationToken);
    }
}