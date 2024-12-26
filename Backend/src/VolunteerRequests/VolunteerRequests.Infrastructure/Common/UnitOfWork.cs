using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetProject.Core.Database;
using VolunteerRequests.Infrastructure.DbContexts;

namespace VolunteerRequests.Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly VolunteerRequestsDbContext _dbContext;

    public UnitOfWork(VolunteerRequestsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}