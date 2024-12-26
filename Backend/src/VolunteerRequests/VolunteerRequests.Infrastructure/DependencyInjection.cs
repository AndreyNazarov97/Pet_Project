using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Common;
using PetProject.Core.Database;
using PetProject.Core.Database.Repository;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Interfaces;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Infrastructure.Common;
using VolunteerRequests.Infrastructure.DbContexts;
using VolunteerRequests.Infrastructure.Repositories;

namespace VolunteerRequests.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerRequestsPostgresInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDatabase()
            .AddDbContext()
            .AddRepositories();
        
        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services
            .AddScoped<VolunteerRequestsDbContext>()
            .AddScoped<IReadDbContext, ReadDbContext>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services
            .AddSingleton<IPostgresConnectionFactory, PostgresConnectionFactory>()
            .AddScoped<IDateTimeProvider, DateTimeProvider>()
            .AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.Context.VolunteerRequests);

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRequestsRepository, VolunteerVolunteerRequestsRepository>();
        services.AddScoped<IReadRepository, ReadRepository>();

        return services;
    }
}