namespace PetProject.Application.Models;

public class VolunteerQueryModel
{
    public Guid[] VolunteerIds { get; init; } = [];
    public Guid[] PetIds { get; init; } = [];
    public Guid[] SpeciesIds { get; init; } = [];
    public Guid[] BreedIds { get; init; } = [];
    public string PhoneNumber { get; init; } = string.Empty;
    public int Limit { get; init; } 
    public int Offset { get; init; }
}