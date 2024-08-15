using FluentValidation;
using PetProject.Domain.Entities;
using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Result;

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
            new SocialNetwork(s.Title, s.Link)).ToList();
        var requisites = request.Requisites.Select(r =>
            new Requisite(r.Title, r.Description)).ToList();

        var volunteer = new Volunteer(
            new FullName(request.FirstName, request.LastName, request.Patronymic),
            request.Description,
            request.Experience,
            0, // TODO логика подсчета животных
            0,
            0,
            request.PhoneNumber,
            socialNetworks,
            requisites);
        
        return await _storage.CreateVolunteer(volunteer, cancellationToken);
    }
}