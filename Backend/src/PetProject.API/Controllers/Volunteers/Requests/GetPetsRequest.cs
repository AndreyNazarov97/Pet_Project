using PetProject.Application.VolunteersManagement.GetPets;
using PetProject.Domain.VolunteerManagement.Enums;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record GetPetsRequest
{
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
    
    public GetPetsQuery ToQuery() => new()
    {
        VolunteerId = VolunteerId,
        Name = Name,
        MinAge = MinAge,
        Breed = Breed,
        Species = Species,
        HelpStatus = HelpStatus,
        SortBy = SortBy,
        SortDescending = SortDescending,
        Limit = Limit,
        Offset = Offset
    };
}