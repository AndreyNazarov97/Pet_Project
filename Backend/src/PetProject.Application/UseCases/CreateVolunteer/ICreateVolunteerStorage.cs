using PetProject.Domain.Entities;

namespace PetProject.Application.UseCases.CreateVolunteer;

public interface ICreateVolunteerStorage
{
    Task<VolunteerId> CreateVolunteer(Volunteer volunteer, CancellationToken cancellationToken);
}