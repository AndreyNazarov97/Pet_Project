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
        var user = await _context.Users
            .Include(u => u.Roles)
                .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rolePermission => rolePermission.Permission)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Array.Empty<Permission>();
        
        var permissions = user.Roles
            .SelectMany(r => r.RolePermissions.Select(rp => rp.Permission)).ToList();

        
        return permissions.AsReadOnly();
    }
}