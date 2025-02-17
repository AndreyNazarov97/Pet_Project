using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Common;
using PetProject.Core.Database;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.Core.Messaging;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Interfaces;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Infrastructure.BackgroundServices;
using PetProject.VolunteerManagement.Infrastructure.Common;
using PetProject.VolunteerManagement.Infrastructure.DataSeed;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;
using PetProject.VolunteerManagement.Infrastructure.MessageQueues;
using PetProject.VolunteerManagement.Infrastructure.Repositories;
using PetProject.VolunteerManagement.Infrastructure.Services;

namespace PetProject.VolunteerManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerPostgresInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDatabase()
            .AddDbContext()
            .AddRepositories()
            .AddHostedServices();

        services.AddSingleton<VolunteersSeeder>();
        services.AddScoped<VolunteersSeedService>();
        
        return services;
    }
    
    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddScoped<VolunteerDbContext>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services
            .AddSingleton<IPostgresConnectionFactory, PostgresConnectionFactory>()
            .AddScoped<IDateTimeProvider, DateTimeProvider>()
            .AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.Context.VolunteerManagement);

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        services.AddScoped<IReadRepository, ReadRepository>();

        return services;
    }
    
    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IMessageQueue<IEnumerable<FileMetaDataDto>>,
                InMemoryMessageQueue<IEnumerable<FileMetaDataDto>>>();


        services.AddScoped<DeleteExpiredPetsService>();
        services.AddScoped<DeleteExpiredVolunteersService>();
        
        services.AddHostedService<DeletedEntityCleanupService>();

        return services;
    }
}