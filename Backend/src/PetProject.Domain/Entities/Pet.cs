using System.ComponentModel.DataAnnotations;
using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Enums;

namespace PetProject.Domain.Entities;

public class Pet : Entity
{
       
    public string Name { get; private set; }

    
    public string Type { get; private set; }
    
    public string Description { get; private set; }
    
    public string Breed { get; private set; }
    
    public string Color { get; private set; }
    
    public string HealthInfo { get; private set; }
    
    public Adress Address { get; private set; }
    
    public int Weight { get; private set; }
    
    public int Height { get; private set; }
    
    public PhoneNumber OwnerPhoneNumber { get; private set; }
    
    public bool IsCastrated { get; private set; }
    
    public DateTimeOffset BirthDate { get; private set; }
    
    public bool IsVaccinated { get; private set; }
    
    public HelpStatus HelpStatus { get; private set; }
    
    public List<Requisite> Requisites { get; private set; } = []; 
    
    public List<PetPhoto> Photos { get; private set; } =[];
    
    public DateTimeOffset CreatedAt { get; private set; }

    private Pet()
    {
        
    }
}