using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetProject.Core.Common;
using PetProject.Core.Database;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.Core.Messaging;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Interfaces;
using PetProject.VolunteerManagement.Application.Providers;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Infrastructure.BackgroundServices;
using PetProject.VolunteerManagement.Infrastructure.Common;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;
using PetProject.VolunteerManagement.Infrastructure.MessageQueues;
using PetProject.VolunteerManagement.Infrastructure.Options;
using PetProject.VolunteerManagement.Infrastructure.Providers;
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
            .AddHostedServices()
            .AddMinio(configuration);

        
        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services
            .AddScoped<VolunteerDbContext>();

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
        
        services.AddHostedService<FilesCleanerBackgroundService>();
        services.AddHostedService<DeletedEntityCleanupService>();

        return services;
    }

    private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.Minio));

        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.Minio).Get<MinioOptions>()
                               ?? throw new AggregateException("Missing minio configuration");

            options.WithEndpoint(minioOptions.Endpoint);

            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }
}