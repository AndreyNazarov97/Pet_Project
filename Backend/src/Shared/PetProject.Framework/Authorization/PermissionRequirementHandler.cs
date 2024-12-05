using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

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
        //получить id пользователя из клеймов
        //получить пользователя по id из базы данных
        //проверить что у пользователя есть нужное разрешение
        
        var scope = _scopeFactory.CreateScope();
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