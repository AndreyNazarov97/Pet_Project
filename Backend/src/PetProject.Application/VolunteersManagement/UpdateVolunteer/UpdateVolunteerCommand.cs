using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.UpdateVolunteer;

public record UpdateVolunteerCommand : IRequest<Result<Guid, Error>>
{
    public required Guid IdVolunteer { get; init; } 
    public required FullNameDto FullName { get; init; } 
    public required string Description { get; init; } 
    public required int AgeExperience { get; init; } 
    public required string PhoneNumber { get; init; } 
}
    