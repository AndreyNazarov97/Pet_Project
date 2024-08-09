using PetProject.Domain.Entities.ValueObjects;

namespace PetProject.API.Models;

public class CreateVolunteerRequest
{
    public FullName FullName { get; set; }
    
    public string Description { get; set; }
    
    public int Experience { get; set; }
    
    public int PetsAdopted { get; set; }
    
    public int PetsFoundHomeQuantity { get; set; }
    
    public int PetsInTreatment { get; set; }
    
    public PhoneNumber PhoneNumber { get; set; }
    
    public List<SocialNetwork> SocialNetworks { get; set; }
    
    public List<Requisite> Requisites { get; set; }
}