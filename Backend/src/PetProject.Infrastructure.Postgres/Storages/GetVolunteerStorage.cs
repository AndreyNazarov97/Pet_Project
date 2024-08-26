using Microsoft.EntityFrameworkCore;
using PetProject.Application.UseCases.Volunteer.GetVolunteer;
using PetProject.Domain.PetManagement.AggregateRoot;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Infrastructure.Postgres.Storages;

public class GetVolunteerStorage : IGetVolunteerStorage
{
    private readonly PetProjectDbContext _dbContext;

    public GetVolunteerStorage(PetProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Volunteer>> GetById(VolunteerId id, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.Volunteers
            .Where(v => v.Id == id);
        var query = queryable.ToQueryString();

        var volunteer = await queryable.FirstOrDefaultAsync(cancellationToken);
        
        if (volunteer is null)
        {
            return Result<Volunteer>.Failure(Errors.General.NotFound(id));
        }

        return Result<Volunteer>.Success(volunteer);
    }

    public async Task<Result<Volunteer>> GetByPhone(PhoneNumber phoneNumber, CancellationToken cancellationToken)
    {
        var volunteer = await _dbContext.Volunteers
            .Where(v => v.PhoneNumber == phoneNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (volunteer is null)
        {
            return Result<Volunteer>.Failure(Errors.General.NotFound());
        }

        return Result<Volunteer>.Success(volunteer);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}