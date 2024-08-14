using FluentValidation;
using PetProject.Domain.Entities;
using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Results;

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

    public async Task<Result<Guid>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken)
    {
        var validation = await _createVolunteerRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var error = new Error(
                string.Join(", ", validation.Errors.Select(x => x.ErrorCode)),
                string.Join(", ", validation.Errors.Select(x => x.ErrorMessage)));

            return Result<Guid>.Failure(error);
        }

        var socialNetworks = request.SocialNetworks.Select(s =>
            SocialNetwork.Create(s.Title, s.Link)).ToList();
        var requisites = request.Requisites.Select(r =>
            Requisite.Create(r.Title, r.Description)).ToList();
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        if(phoneNumber.IsFailure)
            return Result<Guid>.Failure(phoneNumber.Error);
        var fullName = FullName.Create(request.FirstName, request.LastName, request.Patronymic);
        if(fullName.IsFailure)
            return Result<Guid>.Failure(fullName.Error);

        var volunteer = Volunteer.Create(
            fullName.Value,
            request.Description,
            request.Experience,
            0,
            0,
            0,
            phoneNumber.Value);
        
        if (volunteer.IsSuccess)
        {
            foreach (var requisite in requisites)
            {
                if(requisite.IsSuccess)
                    volunteer.Value.AddRequisite(requisite.Value);
            }

            foreach (var socialNetwork in socialNetworks)
            {
                if(socialNetwork.IsSuccess)
                    volunteer.Value.AddSocialNetwork(socialNetwork.Value);
            }
            return await _storage.CreateVolunteer(volunteer.Value, cancellationToken);
        }
        return Result<Guid>.Failure(volunteer.Error);
    }
}