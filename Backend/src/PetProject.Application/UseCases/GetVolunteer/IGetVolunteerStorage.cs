using PetProject.Domain.Entities;
using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Shared;

namespace PetProject.Application.UseCases.GetVolunteer;

public interface IGetVolunteerStorage
{
    Task<Result<Volunteer>> GetVolunteer(VolunteerId id, CancellationToken cancellationToken);
    Task<Result<Volunteer>> GetByPhone(PhoneNumber phoneNumber, CancellationToken cancellationToken);
}