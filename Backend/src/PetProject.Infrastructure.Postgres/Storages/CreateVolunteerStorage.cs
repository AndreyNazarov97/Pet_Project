using Microsoft.EntityFrameworkCore;
using PetProject.Application.UseCases.CreateVolunteer;
using PetProject.Domain.Entities;

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
        await _dbContext.Set<Volunteer>().AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }
}