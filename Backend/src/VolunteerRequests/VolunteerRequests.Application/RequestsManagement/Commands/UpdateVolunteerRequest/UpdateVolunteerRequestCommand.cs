using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace VolunteerRequests.Application.RequestsManagement.Commands.UpdateVolunteerRequest;

public record UpdateVolunteerRequestCommand : IRequest<UnitResult<ErrorList>>
{
    public required Guid VolunteerRequestId { get; init; }
    public required long UserId { get; init; }
    public required FullNameDto FullName { get; init; } 
    public required string Description { get; init; }
    public required int WorkExperience { get; init; } 
    public required string PhoneNumber { get; init; } 
    public required IEnumerable<SocialNetworkDto> SocialNetworksDto { get; init; }
}
