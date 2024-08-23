using Microsoft.EntityFrameworkCore;
using PetProject.Application.UseCases.GetVolunteer;
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

    public async Task<Result<Volunteer>> GetVolunteer(VolunteerId id, CancellationToken cancellationToken)
    {
        // удалить после решения проблемы с инклудами
        var queryable = _dbContext.Volunteers
            .Where(x => x.Id == id);
        var query = queryable.ToQueryString();
        var queryvolunteer = await queryable.FirstOrDefaultAsync(cancellationToken);
        
        var volunteer = await _dbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
       

        if (volunteer is null)
        {
            return Result<Volunteer>.Failure(Errors.General.NotFound(id));
        }

        return Result<Volunteer>.Success(volunteer);
    }

    public async Task<Result<Volunteer>> GetByPhone(PhoneNumber phoneNumber, CancellationToken cancellationToken)
    {
        var volunteer = await _dbContext.Volunteers
            .Where(x => x.PhoneNumber == phoneNumber)
            .Include(v => v.Pets)
                .ThenInclude(p => p.Photos)
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