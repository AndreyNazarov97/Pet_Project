using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetProject.Core.Database;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;

namespace PetProject.VolunteerManagement.Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly VolunteerDbContext _dbContext;

    public UnitOfWork(VolunteerDbContext dbContext)
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