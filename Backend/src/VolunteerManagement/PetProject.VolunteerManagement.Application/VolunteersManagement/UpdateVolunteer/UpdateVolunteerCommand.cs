using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.UpdateVolunteer;

public record UpdateVolunteerCommand : IRequest<Result<Guid, ErrorList>>
{
    public required Guid VolunteerId { get; init; } 
    public required FullNameDto FullName { get; init; } 
    public required string Description { get; init; } 
    public required int AgeExperience { get; init; } 
    public required string PhoneNumber { get; init; } 
}
    