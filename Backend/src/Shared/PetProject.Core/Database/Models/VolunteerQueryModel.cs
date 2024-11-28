namespace PetProject.Core.Database.Models;

public class VolunteerQueryModel
{
    public Guid[] VolunteerIds { get; init; } = [];
    public Guid[] PetIds { get; init; } = [];
    public string[] SpeciesNames { get; init; } = [];
    public string[] BreedNames { get; init; } = [];
    public string PhoneNumber { get; init; } = string.Empty;
    public string? SortBy { get; init; } 
    public bool SortDescending { get; init; } 
    public int Limit { get; init; } 
    public int Offset { get; init; }
}