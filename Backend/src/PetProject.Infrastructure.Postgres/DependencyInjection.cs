using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.UseCases.Volunteer.CreateVolunteer;
using PetProject.Application.UseCases.Volunteer.GetVolunteer;
using PetProject.Infrastructure.Postgres.Abstractions;
using PetProject.Infrastructure.Postgres.Storages;

namespace PetProject.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static void AddPostgresInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<PetProjectDbContext>()
            .AddScoped<IMomentProvider, MomentProvider>()
            .AddScoped<ICreateVolunteerStorage, CreateVolunteerStorage>()
            .AddScoped<IGetVolunteerStorage, GetVolunteerStorage>();
        
        
    }
}