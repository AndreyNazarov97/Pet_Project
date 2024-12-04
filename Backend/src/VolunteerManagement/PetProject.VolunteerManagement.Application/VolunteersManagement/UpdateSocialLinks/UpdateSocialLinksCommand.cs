using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.UpdateSocialLinks;

public record UpdateSocialLinksCommand : IRequest<Result<Guid, ErrorList>>
{
    public required Guid VolunteerId { get; init; } 
    public required IEnumerable<SocialNetworkDto> SocialLinks { get; init; } 
}