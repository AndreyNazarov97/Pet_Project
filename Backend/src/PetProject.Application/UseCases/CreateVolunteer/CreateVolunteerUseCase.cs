using FluentValidation;
using PetProject.Domain.Entities;
using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Shared;

namespace PetProject.Application.UseCases.CreateVolunteer;

public class CreateVolunteerUseCase : ICreateVolunteerUseCase
{
    private readonly ICreateVolunteerStorage _storage;
    private readonly IValidator<CreateVolunteerRequest> _createVolunteerRequestValidator;

    public CreateVolunteerUseCase(
        ICreateVolunteerStorage storage,
        IValidator<CreateVolunteerRequest> createVolunteerRequestValidator)
    {
        _storage = storage;
        _createVolunteerRequestValidator = createVolunteerRequestValidator;
    }

    public async Task<Result<VolunteerId>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken)
    {
        var validation = await _createVolunteerRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var error = new Error(
                string.Join(", ", validation.Errors.Select(x => x.ErrorCode)),
                string.Join(", ", validation.Errors.Select(x => x.ErrorMessage)));

            return Result<VolunteerId>.Failure(error);
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
        

        var volunteerEntity = Volunteer.Create(
            VolunteerId.NewVolunteerId(),
            fullName.Value,
            request.Description,
            request.Experience,
            phoneNumber.Value,
            details.Value,
            null);

        if (volunteerEntity.IsFailure)
        {
            return Result<VolunteerId>.Failure(volunteerEntity.Error!);
        }

        return await _storage.CreateVolunteer(volunteerEntity.Value, cancellationToken);
    }
}