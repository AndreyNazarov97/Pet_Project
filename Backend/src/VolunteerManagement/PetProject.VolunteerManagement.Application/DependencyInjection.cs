using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Common;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Interfaces;

namespace PetProject.VolunteerManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerManagementApplication(this IServiceCollection services)
    {
        services.AddMediatR(c => c
                .RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly)
                .AddOpenBehavior(typeof(ResultValidationPipelineBehavior<,>)));


        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}