using MassTransit;
using PetProject.Accounts.Application;
using PetProject.Accounts.Infrastructure;
using PetProject.Accounts.Infrastructure.Consumers;
using PetProject.Accounts.Presentation;
using PetProject.Discussions.Application;
using PetProject.Discussions.Infrastructure;
using PetProject.Discussions.Presentation;
using PetProject.SpeciesManagement.Application;
using PetProject.SpeciesManagement.Infrastructure;
using PetProject.VolunteerManagement.Application;
using PetProject.VolunteerManagement.Infrastructure;
using VolunteerRequests.Application;
using VolunteerRequests.Infrastructure;

namespace PetProject.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();

            configure.AddConsumer<VolunteerRequestApprovedConsumer>();
            configure.AddConsumer<VolunteerManagement.Infrastructure.Consumers.VolunteerRequestApprovedConsumer>();

            configure.AddConfigureEndpointsCallback((context, cfg) =>
            {
                cfg.UseMessageRetry(r => 
                    r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5)));
            });

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["RabbitMq:Host"]!), h =>
                {
                    h.Username(configuration["RabbitMq:Username"]!);
                    h.Password(configuration["RabbitMq:Password"]!);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAccountsModule(configuration)
            .AddVolunteersModule(configuration)
            .AddVolunteerRequestsModule()
            .AddDiscussionsModule()
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
            .AddVolunteerManagementApplication(configuration)
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

    private static IServiceCollection AddVolunteerRequestsModule(this IServiceCollection services)
    {
        services
            .AddVolunteerRequestsPostgresInfrastructure()
            .AddVolunteerRequestsApplication();

        return services;
    }

    public static IServiceCollection AddDiscussionsModule(this IServiceCollection services)
    {
        services
            .AddDiscussionsPresentation()
            .AddDiscussionsManagementApplication()
            .AddDiscussionsInfrastructure();

        return services;
    }
}