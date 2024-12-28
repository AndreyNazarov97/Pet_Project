using Microsoft.EntityFrameworkCore;
using PetProject.Accounts.Domain;
using PetProject.Accounts.Infrastructure.DbContexts;

namespace PetProject.Accounts.Infrastructure.IdentityManagers;

public class RolePermissionManager
{
    private readonly AccountsDbContext _context;

    public RolePermissionManager(
        AccountsDbContext context)
    {
        _context = context;
    }

    public async Task AddPermissionsToRole(
        string roleName,
        IEnumerable<string> permissions, 
        CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);

        foreach (var permissionCode in permissions)
        {
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Code == permissionCode, cancellationToken);

            if (permission == null)
                throw new ArgumentException($"Permission code {permissionCode} is not found");
            
            var isRolePermissionExist = await _context.RolePermissions
                .AnyAsync(rp => rp.RoleId == role!.Id
                                && rp.PermissionId == permission!.Id,
                    cancellationToken);

            if (isRolePermissionExist)
                continue;

            await _context.RolePermissions.AddAsync(new RolePermission()
            {
                Role = role!,
                Permission = permission!,
                RoleId = role!.Id,
                PermissionId = permission!.Id
            }, cancellationToken);
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}