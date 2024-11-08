using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PetProject.Infrastructure.Authorization;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthorizationInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<AuthorizationDbContext>();
        
        services
            .AddIdentity<User, Role>()
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false
                };
            });
        services.AddAuthorization();

        return services;
    }
}