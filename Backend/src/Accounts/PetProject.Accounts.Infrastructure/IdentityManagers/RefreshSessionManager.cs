using Microsoft.EntityFrameworkCore;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;
using PetProject.Accounts.Infrastructure.DbContexts;

namespace PetProject.Accounts.Infrastructure.IdentityManagers;

public class RefreshSessionManager : IRefreshSessionManager
{
    private readonly AccountsDbContext _context;

    public RefreshSessionManager(
        AccountsDbContext context)
    {
        _context = context;
    }   
    
    public async Task<RefreshSession?> GetByRefreshTokenAsync(Guid refreshToken, CancellationToken cancellationToken)
    {
        return await _context.RefreshSessions.FirstOrDefaultAsync(rs => rs.RefreshToken == refreshToken, cancellationToken);
    }

    public void Delete(RefreshSession refreshSession)
    {
        _context.RefreshSessions.Remove(refreshSession);
    }
}