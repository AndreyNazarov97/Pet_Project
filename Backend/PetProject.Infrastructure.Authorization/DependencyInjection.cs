using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.AccountManagement.DataModels;

namespace PetProject.Infrastructure.Authorization;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthorizationInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<AuthorizationDbContext>();
        
        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
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