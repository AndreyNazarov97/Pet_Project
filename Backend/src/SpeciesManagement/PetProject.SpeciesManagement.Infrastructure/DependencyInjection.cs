using Dapper;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Common;
using PetProject.Core.Database;
using PetProject.Core.Database.Repository;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Interfaces;
using PetProject.SpeciesManagement.Application.Repository;
using PetProject.SpeciesManagement.Infrastructure.Common;
using PetProject.SpeciesManagement.Infrastructure.DbContexts;
using PetProject.SpeciesManagement.Infrastructure.Repositories;

namespace PetProject.SpeciesManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesManagementInfrastructure(this IServiceCollection services)
    {
        services
            .AddDatabase()
            .AddDbContext()
            .AddRepositories();
        
        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        services
            .AddScoped<SpeciesDbContext>();

        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services
            .AddSingleton<IPostgresConnectionFactory, PostgresConnectionFactory>()
            .AddScoped<IDateTimeProvider, DateTimeProvider>()
            .AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.Context.SpeciesManagement);
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<IReadRepository, ReadRepository>();

        return services;
    }
}