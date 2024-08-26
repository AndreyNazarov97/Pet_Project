using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.UseCases.Volunteer.CreateVolunteer;

public interface ICreateVolunteerStorage
{
    Task<VolunteerId> CreateVolunteer(Domain.PetManagement.AggregateRoot.Volunteer volunteer, CancellationToken cancellationToken);
}