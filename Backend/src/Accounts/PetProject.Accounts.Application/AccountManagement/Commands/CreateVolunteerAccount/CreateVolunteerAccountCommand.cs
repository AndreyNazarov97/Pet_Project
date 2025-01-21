using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.CreateVolunteerAccount;

public record CreateVolunteerAccountCommand : IRequest<Result<long, ErrorList>>
{
    public required long UserId { get; init; }
    public required int Experience { get; init; }
}