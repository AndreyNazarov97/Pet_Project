using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetProject.Application.Abstractions;
using PetProject.Application.SpeciesManagement;
using PetProject.Application.Volunteers;
using PetProject.Infrastructure.Postgres.Abstractions;
using PetProject.Infrastructure.Postgres.Providers;
using PetProject.Infrastructure.Postgres.Repositories;
using MinioOptions = PetProject.Infrastructure.Postgres.Options.MinioOptions;

namespace PetProject.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static void AddPostgresInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<PetProjectDbContext>()
            .AddScoped<IMomentProvider, MomentProvider>()
            .AddScoped<IVolunteersRepository, VolunteersRepository>()
            .AddScoped<ISpeciesRepository, SpeciesRepository>()
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