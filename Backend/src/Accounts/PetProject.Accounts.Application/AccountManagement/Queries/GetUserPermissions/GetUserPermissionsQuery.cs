using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Accounts.Domain;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Queries.GetUserPermissions;

public record GetUserPermissionsQuery : IRequest<Result<Permission[], ErrorList>>
{
    public required long UserId { get; init; }
}