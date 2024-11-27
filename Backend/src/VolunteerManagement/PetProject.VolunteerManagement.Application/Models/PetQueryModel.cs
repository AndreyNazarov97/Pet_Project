using PetProject.VolunteerManagement.Domain.Enums;

namespace PetProject.VolunteerManagement.Application.Models;

public class PetQueryModel
{
    public Guid? PetId { get; init; }
    public Guid? VolunteerId{ get; init; } 
    public string? Name { get; init; }
    public int? MinAge { get; init; }
    public string? Breed { get; init; }
    public string? Species { get; init; }
    public HelpStatus? HelpStatus { get; init; }
    public string? SortBy { get; init; } 
    public bool SortDescending { get; init; } 
    public int Limit { get; init; } 
    public int Offset { get; init; }
}
