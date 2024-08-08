namespace PetProject.Domain.Entities;

public class Volunteer : Entity
{
    public string FullName { get; }
    
    public string Description { get; }
    
    public int Experience { get; }
    
    public int PetsAdopted { get; }
    
    public int PetsFoundHomeQuantity { get; }
    
    public int PetsInTreatment { get; }
    
    public string PhoneNumber { get; }
    
    public List<SocialNetwork> SocialNetworks { get; }
    
    public List<Requisite> Requisites { get; }
    
}