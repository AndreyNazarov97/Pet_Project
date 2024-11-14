using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.SpeciesManagement.CreateBreed;
using PetProject.Application.SpeciesManagement.CreateSpecies;
using PetProject.Application.Volunteers.AddPetPhoto;
using PetProject.Application.Volunteers.CreatePet;
using PetProject.Application.Volunteers.CreateVolunteer;
using PetProject.Application.Volunteers.DeleteVolunteer;
using PetProject.Application.Volunteers.UpdateRequisites;
using PetProject.Application.Volunteers.UpdateSocialLinks;
using PetProject.Application.Volunteers.UpdateVolunteer;

namespace PetProject.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateSpeciesHandler>();
        services.AddScoped<CreateBreedHandler>();
        services.AddScoped<CreatePetHandler>();
        services.AddScoped<AddPetPhotoHandler>();
        
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateVolunteerHandler>();
        services.AddScoped<UpdateSocialLinksHandler>();
        services.AddScoped<UpdateRequisitesHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}