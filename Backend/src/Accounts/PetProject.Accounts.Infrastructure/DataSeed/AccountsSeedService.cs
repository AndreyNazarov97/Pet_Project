using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;
using PetProject.Accounts.Infrastructure.IdentityManagers;
using PetProject.Accounts.Infrastructure.Options;
using PetProject.Core.Database;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Infrastructure.DataSeed;

public class AccountsSeedService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IPermissionManager _permissionManager;
    private readonly RolePermissionManager _rolePermissionManager;
    private readonly IAccountManager _accountManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AdminOptions _adminOptions;

    public AccountsSeedService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IPermissionManager permissionManager,
        RolePermissionManager rolePermissionManager,
        IOptions<AdminOptions> adminOptions,
        IAccountManager accountManager,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _permissionManager = permissionManager;
        _rolePermissionManager = rolePermissionManager;
        _accountManager = accountManager;
        _unitOfWork = unitOfWork;
        _adminOptions = adminOptions.Value;
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

        await SeedAdmin();
    }

    private async Task SeedAdmin()
    {
        var isAdminExist = await _userManager.FindByNameAsync(_adminOptions.UserName);

        if (isAdminExist != null)
            return;
        
        var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var adminRole = await _roleManager.FindByNameAsync(AdminAccount.Admin)
                            ?? throw new ApplicationException("Admin role is not found");

            var adminName = FullName.Create(_adminOptions.UserName, _adminOptions.UserName).Value;
            var adminUser = User.CreateAdmin(adminName, _adminOptions.UserName, _adminOptions.Email, adminRole);
            await _userManager.CreateAsync(adminUser, _adminOptions.Password);
        
            var adminAccount = new AdminAccount(adminUser);

            await _accountManager.CreateAdminAccount(adminAccount);
        
            adminUser.AdminAccount = adminAccount;
            adminUser.AdminAccountId = adminAccount.Id;

            transaction.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
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