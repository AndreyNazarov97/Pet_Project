using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetProject.Core.Common;
using PetProject.Core.Database;
using PetProject.SharedKernel.Interfaces;
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
    public static void AddPostgresInfrastructure(this IServiceCollection services)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        services
            .AddSingleton<IPostgresConnectionFactory, PostgresConnectionFactory>()
            .AddScoped<PetProjectDbContext>()
            .AddScoped<IMomentProvider, MomentProvider>()
            .AddScoped<IVolunteersRepository, VolunteersRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static IServiceCollection AddMinioInfrastructure(this IServiceCollection services, IConfiguration configuration)
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