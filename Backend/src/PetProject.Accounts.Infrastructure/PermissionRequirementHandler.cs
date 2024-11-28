using Microsoft.AspNetCore.Authorization;

namespace PetProject.Accounts.Infrastructure;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        var userPermission = context.User.Claims.FirstOrDefault(c => c.Type == "Permission");
        if (userPermission is null)
        {
            return;
        }
        
        if (userPermission.Value == permission.Code)
        {
            context.Succeed(permission);
        }
    }
}