using PetProject.Application.UseCases.Volunteer.CreateVolunteer;
using PetProject.Domain.PetManagement.AggregateRoot;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Infrastructure.Postgres.Storages;

public class CreateVolunteerStorage : ICreateVolunteerStorage
{
    private readonly PetProjectDbContext _dbContext;

    public CreateVolunteerStorage(
        PetProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VolunteerId> CreateVolunteer(Volunteer volunteer, CancellationToken cancellationToken)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }
}