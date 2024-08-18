using Microsoft.EntityFrameworkCore;
using PetProject.Application.UseCases;
using PetProject.Application.UseCases.GetVolunteer;
using PetProject.Domain.Entities;
using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Shared;

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
        return await _dbContext
            .Volunteers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? 
               Result<Volunteer>.Failure(Errors.General.NotFound(id));
    }
    
    public async Task<Result<Volunteer>> GetByPhone(PhoneNumber phoneNumber, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Volunteers
            .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken) ?? 
               Result<Volunteer>.Failure(Errors.General.NotFound());
    }
}