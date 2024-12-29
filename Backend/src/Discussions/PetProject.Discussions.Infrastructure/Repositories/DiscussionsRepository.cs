using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetProject.Discussions.Application.Interfaces;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.Discussions.Infrastructure.DbContexts;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Infrastructure.Repositories;

public class DiscussionsRepository : IDiscussionsRepository
{
    private readonly DiscussionsDbContext _context;

    public DiscussionsRepository(DiscussionsDbContext context)
    {
        _context = context;
    }
    
    public async Task<Discussion> Add(Discussion discussion, CancellationToken cancellationToken = default)
    {
        await _context.Discussions.AddAsync(discussion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return discussion;
    }

    public async Task<Result<Guid, Error>> Delete(Discussion discussion, CancellationToken cancellationToken = default)
    {
        _context.Discussions.Remove(discussion);
        await _context.SaveChangesAsync(cancellationToken);
        return discussion.Id.Id;
    }

    public async Task<Result<Discussion, Error>> GetByRelationId(Guid relationId, CancellationToken cancellationToken = default)
    {
        var discussion = _context.Discussions.FirstOrDefault(d => d.RelationId == relationId);
        if (discussion == null)
            return Errors.General.NotFound(relationId);

        return discussion;
    }
}