using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Common;
using PetProject.Core.Database;
using PetProject.Core.Database.Repository;
using PetProject.Discussions.Application.Interfaces;
using PetProject.Discussions.Infrastructure.Common;
using PetProject.Discussions.Infrastructure.DbContexts;
using PetProject.Discussions.Infrastructure.Repositories;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Interfaces;

namespace PetProject.Discussions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionsInfrastructure(this IServiceCollection services)
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
            .AddScoped<IReadDbContext,ReadDbContext>()
            .AddScoped<DiscussionsDbContext>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services
            .AddSingleton<IPostgresConnectionFactory, PostgresConnectionFactory>()
            .AddScoped<IDateTimeProvider, DateTimeProvider>()
            .AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.Context.Discussions);

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        
        services.AddScoped<IDiscussionsRepository, DiscussionsRepository>();
        services.AddScoped<IReadRepository, ReadRepository>();

        return services;
    }
}