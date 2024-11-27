using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Validation;

namespace PetProject.VolunteerManagement.Application;

public static class DependencyInjection
{
    public static void AddVolunteerApplication(this IServiceCollection services)
    {
        services.AddMediatR(c => c
                .RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly)
                .AddOpenBehavior(typeof(ResultValidationPipelineBehavior<,>)));
        
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}