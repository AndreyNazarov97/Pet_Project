using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.Volunteers;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement;

namespace PetProject.Infrastructure.Postgres.Repositories;

public class VolunteersRepository(PetProjectDbContext context) : IVolunteersRepository
{
    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await context.Volunteers.AddAsync(volunteer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Guid, Error>> Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
        return volunteer.Id.Id;
    }

    public async Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber requestNumber,
        CancellationToken cancellationToken = default)
    {
        var volunteer =
            await context.Volunteers.FirstOrDefaultAsync(v => v.PhoneNumber == requestNumber, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default)
    {
        var volunteer = await context.Volunteers
            .Where(v => v.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound(id.Id);

        return volunteer;
    }
}