﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Application.UseCases.CreateVolunteer;
using PetProject.Application.UseCases.GetVolunteer;
using PetProject.Infrastructure.Postgres.Abstractions;
using PetProject.Infrastructure.Postgres.Storages;

namespace PetProject.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static void AddPostgresInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IMomentProvider, MomentProvider>()
            .AddScoped<ICreateVolunteerStorage, CreateVolunteerStorage>()
            .AddScoped<IGetVolunteerStorage, GetVolunteerStorage>();
        
        
        var connectionString = configuration.GetConnectionString("Postgres");

        services.AddDbContext<PetProjectDbContext>(options =>
        {
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
                .UseLoggerFactory(CreateLoggerFactory())
                .EnableSensitiveDataLogging();
        });
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}