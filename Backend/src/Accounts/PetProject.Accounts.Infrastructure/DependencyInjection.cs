using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetProject.Accounts.Application;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;
using PetProject.Accounts.Infrastructure.Common;
using PetProject.Accounts.Infrastructure.DataSeed;
using PetProject.Accounts.Infrastructure.IdentityManagers;
using PetProject.Accounts.Infrastructure.Options;
using PetProject.Accounts.Infrastructure.Providers;
using PetProject.Core.Database;
using PetProject.Framework.Authorization;
using PetProject.SharedKernel.Constants;

namespace PetProject.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.Admin));

        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        services.AddScoped<IPostgresConnectionFactory, PostgresConnectionFactory>();

        services.AddScoped<AccountsDbContext>();
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.Context.Accounts);

        services.RegisterIdentity();

        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeedService>();

        services.RegisterAuthentication(configuration);
        services.RegisterAuthorization();

        return services;
    }

    private static void RegisterIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AccountsDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IPermissionManager, PermissionManager>();
        services.AddScoped<IAccountManager, AccountManager>();
        services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();
        services.AddScoped<RolePermissionManager>();
    }

    private static void RegisterAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
    }

    private static void RegisterAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing jwt configuration");

                options.TokenValidationParameters = TokenValidationParametersFactory.Create(jwtOptions);
            });
    }
}