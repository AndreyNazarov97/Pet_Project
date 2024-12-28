using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Domain.Aggregate;
using VolunteerRequests.Infrastructure.DbContexts;

namespace VolunteerRequests.Infrastructure.Repositories;

public class VolunteerRequestsRepository : IVolunteerRequestsRepository
{
    private readonly VolunteerRequestsDbContext _context;

    public VolunteerRequestsRepository(VolunteerRequestsDbContext context)
    {
        _context = context;
    }


    public async Task<Guid> Add(VolunteerRequest request, CancellationToken cancellationToken = default)
    {
        await _context.VolunteerRequests.AddAsync(request, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }

    public async Task<Result<Guid, Error>> Delete(VolunteerRequest request, CancellationToken cancellationToken = default)
    {
        _context.VolunteerRequests.Remove(request);
        await _context.SaveChangesAsync(cancellationToken);
        return request.Id.Id;
    }

    public async Task<Result<VolunteerRequest, Error>> GetById(VolunteerRequestId id, CancellationToken cancellationToken = default)
    {
        var request = await _context.VolunteerRequests
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (request == null)
            return Errors.General.NotFound(id.Id);

        return request;
    }
}