using Microsoft.EntityFrameworkCore;
using PetProject.Accounts.Domain;

namespace PetProject.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager
{
    private readonly AccountsDbContext _context;

    public PermissionManager(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<Permission?> FindByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await _context.Permissions
            .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
    } 
    
    public async Task AddRangeIfExist(IEnumerable<string> permissions, CancellationToken cancellationToken)
    {
        foreach (var permissionCode in permissions)
        {
            var isPermissionExist = await _context.Permissions.AnyAsync(p =>
                p.Code == permissionCode, cancellationToken);

            if (isPermissionExist)
                return;

            _context.Permissions.Add(new Permission() { Code = permissionCode });
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}