using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.Authorization.Commands.LoginUser;

public record LoginUserCommand() : IRequest<Result<string,ErrorList>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}