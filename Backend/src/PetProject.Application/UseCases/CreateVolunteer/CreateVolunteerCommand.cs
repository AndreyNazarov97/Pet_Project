using PetProject.Domain.Entities.ValueObjects;

namespace PetProject.Application.UseCases.CreateVolunteer;

public record CreateVolunteerCommand(
    FullName FullName, 
    string Description, 
    int Experience,
    int PetsAdopted,
    int PetsFoundHomeQuantity,
    int PetsInTreatment,
    PhoneNumber PhoneNumber,
    List<Requisite> Requisites,
    List<SocialNetwork> SocialNetworks
    );