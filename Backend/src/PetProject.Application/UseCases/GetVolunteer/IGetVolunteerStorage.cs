using PetProject.Domain.PetManagement.AggregateRoot;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.UseCases.GetVolunteer;

public interface IGetVolunteerStorage
{
    Task<Result<Volunteer>> GetVolunteer(VolunteerId id, CancellationToken cancellationToken);
    Task<Result<Volunteer>> GetByPhone(PhoneNumber phoneNumber, CancellationToken cancellationToken);
}