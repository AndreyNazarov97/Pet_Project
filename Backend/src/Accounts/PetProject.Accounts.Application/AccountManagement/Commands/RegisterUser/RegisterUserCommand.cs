using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.RegisterUser;

public record RegisterUserCommand() : IRequest<UnitResult<ErrorList>>
{
    public required string Email { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
    
    public required FullNameDto FullName { get; init; }
}