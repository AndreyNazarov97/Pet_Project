using PetProject.Domain.Entities;

namespace PetProject.Application.UseCases.CreateVolunteer;

public interface ICreateVolunteerStorage
{
    Task<Guid> CreateVolunteer(Volunteer volunteer, CancellationToken cancellationToken);
}