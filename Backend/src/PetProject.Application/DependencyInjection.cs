using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.UseCases.Volunteer.CreateVolunteer;
using PetProject.Application.UseCases.Volunteer.UpdateMainInfo;
using PetProject.Application.UseCases.Volunteer.UpdateRequisites;
using PetProject.Application.UseCases.Volunteer.UpdateSocialNetworks;

namespace PetProject.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateVolunteerUseCase, CreateVolunteerUseCase>()
            .AddScoped<IUpdateMainInfoUseCase, UpdateMainInfoUseCase>()
            .AddScoped<IUpdateSocialNetworksUseCase, UpdateSocialNetworksUseCase>()
            .AddScoped<IUpdateRequisitesUseCase, UpdateRequisitesUseCase>();


        services.AddValidatorsFromAssemblyContaining<CreateVolunteerUseCase>();
    }
}