using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.CreateVolunteer;

public record CreateVolunteerCommand : IRequest<Result<Guid, ErrorList>>
{
    public required FullNameDto FullName { get; init; } 
    public required string Description { get; init; }
    public required int AgeExperience { get; init; } 
    public required string PhoneNumber { get; init; } 
    public required IEnumerable<SocialLinkDto> SocialLinks { get; init; }
    public required IEnumerable<RequisiteDto> Requisites { get; init; } 
}