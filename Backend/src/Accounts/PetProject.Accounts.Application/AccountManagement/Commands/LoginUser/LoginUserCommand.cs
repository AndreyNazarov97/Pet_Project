using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Accounts.Contracts.Responses;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.LoginUser;

public record LoginUserCommand() : IRequest<Result<LoginResponse,ErrorList>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}