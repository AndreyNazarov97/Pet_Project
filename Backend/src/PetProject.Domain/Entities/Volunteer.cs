namespace PetProject.Domain.Entities;

public class Volunteer
{
    public Guid Id { get; }
    
    public string FullName { get; }
    
    public string Description { get; }
    
    public int Experience { get; }
    
    public int PetsAdopted { get; }
    
    public string PhoneNumber { get; }
    
    public List<string> SocialNetworks { get; }
    
    public List<Requisite> Requisites { get; }
    
}