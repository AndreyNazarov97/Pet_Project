using PetProject.Domain.Entities;
using PetProject.Domain.Result;

namespace PetProject.Application.UseCases.CreateVolunteer;

public interface ICreateVolunteerUseCase
{
    Task<Result<VolunteerId>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken);
}