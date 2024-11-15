using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.UpdateVolunteer;

public record UpdateVolunteerCommand
{
    public required Guid IdVolunteer { get; init; } 
    public required FullNameDto FullName { get; init; } 
    public required string Description { get; init; } 
    public required int AgeExperience { get; init; } 
    public required string PhoneNumber { get; init; } 
}
    