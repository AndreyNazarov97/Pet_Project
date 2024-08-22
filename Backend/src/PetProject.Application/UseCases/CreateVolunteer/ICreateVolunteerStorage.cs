using PetProject.Domain.PetManagement.AggregateRoot;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.UseCases.CreateVolunteer;

public interface ICreateVolunteerStorage
{
    Task<VolunteerId> CreateVolunteer(Volunteer volunteer, CancellationToken cancellationToken);
}