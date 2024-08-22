using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.UseCases.CreateVolunteer;

public interface ICreateVolunteerUseCase
{
    Task<Result<VolunteerId>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken);
}