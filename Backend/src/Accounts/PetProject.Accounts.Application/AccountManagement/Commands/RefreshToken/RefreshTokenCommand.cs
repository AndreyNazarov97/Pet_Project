using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Accounts.Contracts.Responses;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.RefreshToken;

public record RefreshTokenCommand : IRequest<Result<LoginResponse, ErrorList>>
{
    public required Guid RefreshToken { get; init; }
}