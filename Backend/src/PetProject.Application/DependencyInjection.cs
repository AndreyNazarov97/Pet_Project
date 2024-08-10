using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.Models;
using PetProject.Application.UseCases.CreateVolunteer;
using PetProject.Application.Validators;

namespace PetProject.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateVolunteerUseCase, CreateVolunteerUseCase>();
        
        
        
        AddValidators(services);
    }
    
    private static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateVolunteerRequest>, CreateVolunteerRequestValidator>();
            
    }
}