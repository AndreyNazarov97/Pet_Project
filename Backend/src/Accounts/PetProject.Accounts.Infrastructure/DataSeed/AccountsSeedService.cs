using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using PetProject.Accounts.Domain;
using PetProject.Accounts.Infrastructure.IdentityManagers;
using PetProject.Accounts.Infrastructure.Options;

namespace PetProject.Accounts.Infrastructure.DataSeed;

public class AccountsSeedService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly PermissionManager _permissionManager;
    private readonly RolePermissionManager _rolePermissionManager;

    public AccountsSeedService(
        RoleManager<Role> roleManager,
        PermissionManager permissionManager,
        RolePermissionManager rolePermissionManager)
    {
        _roleManager = roleManager;
        _permissionManager = permissionManager;
        _rolePermissionManager = rolePermissionManager;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var json = await File.ReadAllTextAsync(
            "Properties/accounts.json",
            cancellationToken);

        var seedData = JsonSerializer.Deserialize<RolePermissionOptions>(json)
                       ?? throw new AggregateException("Missing seed data");

        await SeedPermissions(seedData, cancellationToken);

        await SeedRoles(seedData, cancellationToken);

        await SeedRolePermissions(seedData, cancellationToken);
    }

    private async Task SeedRolePermissions(RolePermissionOptions seedData, CancellationToken cancellationToken)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            await _rolePermissionManager.AddPermissionsToRole(
                roleName,
                seedData.Roles[roleName],
                cancellationToken);
        }
    }

    private async Task SeedRoles(RolePermissionOptions seedData, CancellationToken cancellationToken)
    {
        foreach (var role in seedData.Roles.Keys)
        {
            var existedRole = await _roleManager.FindByNameAsync(role);

            if (existedRole == null)
                await _roleManager.CreateAsync(new Role()
                {
                    Name = role, 
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });
        }
    }

    private async Task SeedPermissions(RolePermissionOptions? seedData,
        CancellationToken cancellationToken)
    {
        var permissions = seedData!.Permissions
            .SelectMany(p => p.Value)
            .Distinct()
            .ToArray();

        await _permissionManager.AddRangeIfExist(permissions, cancellationToken);
    }
}