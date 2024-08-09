using PetProject.Domain.Entities;

namespace PetProject.Application.UseCases.CreateVolunteer;

public interface ICreateVolunteerStorage
{
    Task<Volunteer> CreateVolunteer(CreateVolunteerCommand command, CancellationToken cancellationToken);
}