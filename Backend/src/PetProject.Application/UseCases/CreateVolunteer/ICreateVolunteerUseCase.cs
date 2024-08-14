using PetProject.Domain.Results;

namespace PetProject.Application.UseCases.CreateVolunteer;

public interface ICreateVolunteerUseCase
{
    Task<Result<Guid>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken);
}