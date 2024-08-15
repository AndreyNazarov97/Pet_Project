using PetProject.Domain.Entities.ValueObjects;

namespace PetProject.Domain.Entities;

public class Volunteer : Entity<VolunteerId>
{
    public FullName FullName { get; private set; }
    
    public string Description { get; private set; }
    
    public int Experience { get; private set; }
    
    public int PetsAdopted { get; private set; }
    
    public int PetsFoundHomeQuantity { get; private set; }
    
    public int PetsInTreatment { get; private set; }
    
    public PhoneNumber PhoneNumber { get; private set; }
    
    public List<SocialNetwork> SocialNetworks { get; private set; }
    
    public List<Requisite> Requisites { get; private set; }

    private Volunteer()
    {
    }
    
    public Volunteer(
        FullName fullName, 
        string description, 
        int experience, 
        int petsAdopted, 
        int petsFoundHomeQuantity, 
        int petsInTreatment, 
        PhoneNumber phoneNumber, 
        List<SocialNetwork> socialNetworks, 
        List<Requisite> requisites) 
    {
        FullName = fullName;
        Description = description;
        Experience = experience;
        PetsAdopted = petsAdopted;
        PetsFoundHomeQuantity = petsFoundHomeQuantity;
        PetsInTreatment = petsInTreatment;
        PhoneNumber = phoneNumber;
        SocialNetworks = socialNetworks;
        Requisites = requisites;
    }
}