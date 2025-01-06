using FileService.Communication;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Common;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Interfaces;

namespace PetProject.VolunteerManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerManagementApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(c => c
                .RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly)
                .AddOpenBehavior(typeof(ResultValidationPipelineBehavior<,>)));

        services.AddFileService(configuration);
        
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}