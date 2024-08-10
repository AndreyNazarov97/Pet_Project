using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.UseCases.CreateVolunteer;
using PetProject.Domain.Entities;

namespace PetProject.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateVolunteerUseCase, CreateVolunteerUseCase>();
        
        
       
        services.AddValidatorsFromAssemblyContaining<Volunteer>();
    }
}