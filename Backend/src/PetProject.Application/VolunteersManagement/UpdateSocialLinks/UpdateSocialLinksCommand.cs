using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.UpdateSocialLinks;

public record UpdateSocialLinksCommand : IRequest<Result<Guid, ErrorList>>
{
    public required Guid Id { get; init; } 
    public required IEnumerable<SocialLinkDto> SocialLinks { get; init; } 
}