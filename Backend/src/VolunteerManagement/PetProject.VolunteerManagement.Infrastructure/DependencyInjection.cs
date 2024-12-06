using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetProject.Core.Common;
using PetProject.Core.Database;
using PetProject.Core.Database.Dapper;
using PetProject.Core.Database.Repository;
using PetProject.SharedKernel.Interfaces;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.Providers;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Infrastructure.Common;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;
using PetProject.VolunteerManagement.Infrastructure.Options;
using PetProject.VolunteerManagement.Infrastructure.Providers;
using PetProject.VolunteerManagement.Infrastructure.Repositories;

namespace PetProject.VolunteerManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerPostgresInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabase()
            .AddDbContext()
            .AddRepositories().
            AddMinio(configuration);
        
        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        Dapper.SqlMapper.AddTypeHandler(new JsonTypeHandler<Requisite[]>());
        
        services
            .AddScoped<VolunteerDbContext>()
            .AddScoped<IReadDbContext, ReadDbContext>();

        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services
            .AddSingleton<IPostgresConnectionFactory, PostgresConnectionFactory>()
            .AddScoped<IMomentProvider, MomentProvider>()
            .AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        services.AddScoped<IReadRepository, ReadRepository>();

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