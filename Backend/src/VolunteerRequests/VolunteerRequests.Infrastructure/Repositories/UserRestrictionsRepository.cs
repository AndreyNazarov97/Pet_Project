using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Domain.Aggregate;
using VolunteerRequests.Infrastructure.DbContexts;

namespace VolunteerRequests.Infrastructure.Repositories;

public class UserRestrictionsRepository : IUserRestrictionsRepository
{
    private readonly VolunteerRequestsDbContext _context;

    public UserRestrictionsRepository(VolunteerRequestsDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> Add(UserRestriction restriction, CancellationToken cancellationToken = default)
    {
        await _context.UserRestrictions.AddAsync(restriction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return restriction.Id;
    }

    public async Task<Result<Guid, Error>> Delete(UserRestriction restriction, CancellationToken cancellationToken = default)
    {
        _context.UserRestrictions.Remove(restriction);
        await _context.SaveChangesAsync(cancellationToken);
        return restriction.Id.Id;
    }

    public async Task<Result<UserRestriction, Error>> GetById(UserRestrictionId id, CancellationToken cancellationToken = default)
    {
        var restriction = await _context.UserRestrictions
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (restriction == null)
            return Errors.General.NotFound(id.Id);

        return restriction;
    }

    public async Task<Result<UserRestriction, Error>> GetByUserId(long userId, CancellationToken cancellationToken = default)
    {
        var restriction = await _context.UserRestrictions
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken: cancellationToken);

        if (restriction == null)
            return Errors.General.NotFound(userId);

        return restriction;
    }
}