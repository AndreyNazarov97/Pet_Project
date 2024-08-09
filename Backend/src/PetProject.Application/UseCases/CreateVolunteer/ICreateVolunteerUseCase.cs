using PetProject.Application.Models;

namespace PetProject.Application.UseCases.CreateVolunteer;

public interface ICreateVolunteerUseCase
{
    Task<Guid> Create(CreateVolunteerRequest request, CancellationToken cancellationToken);
}