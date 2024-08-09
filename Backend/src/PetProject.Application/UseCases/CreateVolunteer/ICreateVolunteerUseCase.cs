using PetProject.Domain.Entities;

namespace PetProject.Application.UseCases.CreateVolunteer;

public interface ICreateVolunteerUseCase
{
    Task<Volunteer> Create(CreateVolunteerCommand command, CancellationToken cancellationToken);
}