using PetProject.Application.Models;
using PetProject.Domain.Entities;
using PetProject.Domain.Entities.ValueObjects;

namespace PetProject.Application.UseCases.CreateVolunteer;

public class CreateVolunteerUseCase : ICreateVolunteerUseCase
{
    private readonly ICreateVolunteerStorage _storage;

    public CreateVolunteerUseCase(
        ICreateVolunteerStorage storage)
    {
        _storage = storage;
    }
    public async Task<Guid> Create(CreateVolunteerRequest request, CancellationToken cancellationToken)
    {
        var socialNetworks = request.SocialNetworks.Select(s =>
            new SocialNetwork(s.Title, s.Link)).ToList();
        var requisites = request.Requisites.Select(r =>
            new Requisite(r.Title, r.Description)).ToList();

        var volunteer = new Volunteer(
            new FullName(request.FirstName, request.LastName, request.Patronymic),
            request.Description,
            request.Experience,
            request.PetsAdopted,
            request.PetsFoundHomeQuantity,
            request.PetsInTreatment,
            request.PhoneNumber,
            socialNetworks,
            requisites);
        
        return await _storage.CreateVolunteer(volunteer, cancellationToken);
    }
}