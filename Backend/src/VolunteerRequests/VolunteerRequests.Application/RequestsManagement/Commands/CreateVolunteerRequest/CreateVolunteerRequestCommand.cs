using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;

public record CreateVolunteerRequestCommand : IRequest<Result<Guid, ErrorList>>
{
    public required long UserId { get; init; }
    public required FullNameDto FullName { get; init; } 
    public required string Description { get; init; }
    public required int AgeExperience { get; init; } 
    public required string PhoneNumber { get; init; } 
    public required IEnumerable<SocialNetworkDto> SocialNetworksDto { get; init; }
}