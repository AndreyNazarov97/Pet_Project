using PetProject.Domain.Entities;

namespace PetProject.Application.UseCases.CreateVolunteer;

public class CreateVolunteerUseCase : ICreateVolunteerUseCase
{
    private readonly ICreateVolunteerStorage _storage;

    public CreateVolunteerUseCase(
        ICreateVolunteerStorage storage)
    {
        _storage = storage;
    }
    public async Task<Volunteer> Create(CreateVolunteerCommand command, CancellationToken cancellationToken)
    {
        return await _storage.CreateVolunteer(command, cancellationToken);
    }
}