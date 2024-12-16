using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.Register;

public record RegisterCommand() : IRequest<UnitResult<ErrorList>>
{
    public required string Email { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
    
    public required FullNameDto FullName { get; init; }
}