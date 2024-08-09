namespace PetProject.Domain.Entities;

public class Volunteer : Entity
{
    public string FullName { get; private set; }
    
    public string Description { get; private set; }
    
    public int Experience { get; private set; }
    
    public int PetsAdopted { get; private set; }
    
    public int PetsFoundHomeQuantity { get; private set; }
    
    public int PetsInTreatment { get; private set; }
    
    public string PhoneNumber { get; private set; }
    
    public List<SocialNetwork> SocialNetworks { get; private set; }
    
    public List<Requisite> Requisites { get; private set; }

    private Volunteer()
    {
        
    }
    
}