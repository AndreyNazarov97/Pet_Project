using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerCommand
{
    public required FullNameDto FullName { get; init; } 
    public required string Description { get; init; }
    public required int AgeExperience { get; init; } 
    public required string PhoneNumber { get; init; } 
    public required IEnumerable<SocialLinkDto> SocialLinks { get; init; }
    public required IEnumerable<RequisiteDto> Requisites { get; init; } 
}