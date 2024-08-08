using System.ComponentModel.DataAnnotations;
using PetProject.Domain.Enums;

namespace PetProject.Domain.Entities;

public class Pet
{
    
    public Guid Id { get; }
    
    public string Name { get; }
    
    public string Type { get; }
    
    public string Description { get; }
    
    public string Breed { get; }
    
    public string Color { get; }
    
    public string HealthInfo { get; }
    
    public string Address { get; }
    
    public int Weight { get; }
    
    public int Height { get; }
    
    public string OwnerPhoneNumber { get; }
    
    public bool IsCastrated { get; }
    
    public DateTimeOffset BirthDate { get; }
    
    public bool IsVaccinated { get; }
    
    public HelpStatus HelpStatus { get; }
    
    public List<Requisite> Requisites { get; } = new(); 
    
    public DateTimeOffset CreatedAt { get; }
}