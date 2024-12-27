using PetProject.Accounts.Application;
using PetProject.Accounts.Infrastructure;
using PetProject.Accounts.Presentation;
using PetProject.SpeciesManagement.Application;
using PetProject.SpeciesManagement.Infrastructure;
using PetProject.VolunteerManagement.Application;
using PetProject.VolunteerManagement.Infrastructure;
using VolunteerRequests.Application;
using VolunteerRequests.Infrastructure;

namespace PetProject.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAccountsModule(configuration)
            .AddVolunteersModule(configuration)
            .AddVolunteerRequestsModule(configuration)
            .AddSpeciesModule();

        return services;
    }

    private static IServiceCollection AddAccountsModule(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .AddAccountsManagementApplication()
            .AddAccountsInfrastructure(configuration)
            .AddAccountsPresentation();

        return services;
    }

    private static IServiceCollection AddVolunteersModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddVolunteerManagementApplication()
            .AddVolunteerPostgresInfrastructure(configuration);

        return services;
    }

    private static IServiceCollection AddSpeciesModule(this IServiceCollection services)
    {
        services
            .AddSpeciesManagementApplication()
            .AddSpeciesManagementInfrastructure();

        return services;
    }

    private static IServiceCollection AddVolunteerRequestsModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddVolunteerRequestsPostgresInfrastructure(configuration)
            .AddVolunteerRequestsApplication();

        return services;
    }
}