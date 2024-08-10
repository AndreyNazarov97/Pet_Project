using PetProject.Domain.Result;

namespace PetProject.Application.UseCases.CreateVolunteer;

public interface ICreateVolunteerUseCase
{
    Task<Result<Guid>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken);
}