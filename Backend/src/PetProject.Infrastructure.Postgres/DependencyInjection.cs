using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.SpeciesManagement;
using PetProject.Application.Volunteers;
using PetProject.Infrastructure.Postgres.Abstractions;
using PetProject.Infrastructure.Postgres.Repositories;

namespace PetProject.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static void AddPostgresInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<PetProjectDbContext>()
            .AddScoped<IMomentProvider, MomentProvider>()
            .AddScoped<IVolunteersRepository, VolunteersRepository>()
            .AddScoped<ISpeciesRepository, SpeciesRepository>();
        
        
    }
}