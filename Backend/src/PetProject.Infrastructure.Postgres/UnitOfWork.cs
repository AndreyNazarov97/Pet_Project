using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetProject.Application.Abstractions;

namespace PetProject.Infrastructure.Postgres;

public class UnitOfWork : IUnitOfWork
{
    private readonly PetProjectDbContext _dbContext;

    public UnitOfWork(PetProjectDbContext dbContext)
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