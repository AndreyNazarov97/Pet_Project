using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.Validation;

namespace PetProject.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(c => c
                .RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly)
                .AddOpenBehavior(typeof(ResultValidationPipelineBehavior<,>)));
        
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}