using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos.Accounts;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Queries.GetUserInfo;

public record GetUserInfoQuery : IRequest<Result<UserDto, ErrorList>>
{
    public long UserId { get; init; }
}