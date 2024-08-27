using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.UseCases.Volunteer.GetVolunteer;

public interface IGetVolunteerStorage
{
    Task<Result<Domain.PetManagement.AggregateRoot.Volunteer>> GetById(VolunteerId id, CancellationToken cancellationToken);
    Task<Result<Domain.PetManagement.AggregateRoot.Volunteer>> GetByPhone(PhoneNumber phoneNumber, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}