using Microsoft.Extensions.DependencyInjection;
using PetProject.Discussions.Application.DiscussionsManagement.Commands;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.CreateDiscussion;
using PetProject.Discussions.Contracts;

namespace PetProject.Discussions.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionsPresentation(this IServiceCollection services)
    {
        services.AddScoped<CreateDiscussionHandler>();
        
        services.AddContracts();

        return services;
    }
    
    private static IServiceCollection AddContracts(this IServiceCollection services)
    {
        services.AddScoped<IDiscussionContract, DiscussionContract>();
        
        return services;
    }
}