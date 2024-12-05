using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Contracts;
using PetProject.Accounts.Domain;

namespace PetProject.Framework.Authorization;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PermissionRequirementHandler(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        using var scope = _scopeFactory.CreateScope();

        var accountContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();

        var userIdString = context.User.Claims
            .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (int.TryParse(userIdString, out var userId) == false)
        {
            context.Fail();
            return;
        }

        var permissionsResult = await accountContract.GetUserPermissions(userId);
        if (permissionsResult.IsFailure)
            return;

        var permissions = permissionsResult.Value!;

        if (permissions
            .Select(p => p.Code)
            .Contains(permission.Code))
        {
            context.Succeed(permission);
        }
        
        context.Fail();
    }
}