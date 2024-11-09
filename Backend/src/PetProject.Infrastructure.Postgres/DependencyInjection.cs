using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.VolunteersManagement;
using PetProject.Infrastructure.Postgres.Abstractions;
using PetProject.Infrastructure.Postgres.Repositories;

namespace PetProject.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<PetProjectDbContext>()
            .AddScoped<IMomentProvider, MomentProvider>()
            .AddScoped<IVolunteersRepository, VolunteersRepository>();

        return services;
    }
}