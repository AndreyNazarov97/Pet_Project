using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Application.AccountManagement.Queries.GetUserInfo;
using PetProject.Accounts.Application.AccountManagement.Queries.GetUserPermissions;
using PetProject.Accounts.Contracts;

namespace PetProject.Accounts.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsPresentation(this IServiceCollection services)
    {
        services.AddScoped<GetUserPermissionsHandler>();
        services.AddScoped<GetUserInfoHandler>();
        
        services.AddContracts();

        return services;
    }
    
    private static IServiceCollection AddContracts(this IServiceCollection services)
    {
        services.AddScoped<IAccountsContract, AccountsContract>();
        
        return services;
    }
}