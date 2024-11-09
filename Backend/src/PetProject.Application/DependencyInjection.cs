using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.AccountManagement.Commands;
using PetProject.Application.VolunteersManagement.CreateVolunteer;
using PetProject.Application.VolunteersManagement.DeleteVolunteer;
using PetProject.Application.VolunteersManagement.UpdateRequisites;
using PetProject.Application.VolunteersManagement.UpdateSocialLinks;
using PetProject.Application.VolunteersManagement.UpdateVolunteer;

namespace PetProject.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddVolunteerManagement(services);
        AddAccountManagement(services);
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }

    private static void AddAccountManagement(IServiceCollection services)
    {
        services.AddScoped<RegisterUserHandler>();
    }

    private static void AddVolunteerManagement(IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateVolunteerHandler>();
        services.AddScoped<UpdateSocialLinksHandler>();
        services.AddScoped<UpdateRequisitesHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
    }
}