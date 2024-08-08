using System.ComponentModel.DataAnnotations;
using PetProject.Domain.Enums;

namespace PetProject.Domain.Entities;

public class Pet
{
    [Required]
    public Guid Id { get; }
    [Required]
    public string Name { get; }
    [Required]
    public string Type { get; }
    public string Description { get; }
    [Required]
    public string Breed { get; }
    [Required]
    public string Color { get; }
    [Required]
    public string HealthInfo { get; }
    [Required]
    public string Address { get; }
    [Required]
    public int Weight { get; }
    [Required]
    public int Height { get; }
    [Required]
    public string OwnerPhoneNumber { get; }
    [Required]
    public bool IsCastrated { get; }
    [Required]
    public DateTimeOffset BirthDate { get; }
    [Required]
    public bool IsVaccinated { get; }
    [Required]
    public HelpStatus HelpStatus { get; }
    [Required]
    public Requisite Requisite { get; }
    public DateTimeOffset CreatedAt { get; }

   
}