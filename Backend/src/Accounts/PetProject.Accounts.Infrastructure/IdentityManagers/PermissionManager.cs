using Microsoft.EntityFrameworkCore;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;

namespace PetProject.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager : IPermissionManager
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

    public async Task<IReadOnlyList<Permission>> GetUserPermissionsAsync(long userId,
        CancellationToken cancellationToken)
    {
       var permissions = await _context.RolePermissions
            .Where(rp => _context.UserRoles
                .Any(ur => ur.RoleId == rp.RoleId && ur.UserId == userId))
            .Select(rp => rp.Permission)
            .Distinct()
            .ToListAsync(cancellationToken);
        
        return permissions.AsReadOnly();
    }
}