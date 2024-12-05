using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.UpdateSocialLinks;

public record UpdateSocialLinksCommand : IRequest<Result<long, ErrorList>>
{
    public required long UserId { get; init; } 
    public required IEnumerable<SocialNetworkDto> SocialLinks { get; init; } 
}