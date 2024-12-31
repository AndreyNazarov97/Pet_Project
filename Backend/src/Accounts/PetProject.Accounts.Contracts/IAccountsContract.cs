using CSharpFunctionalExtensions;
using PetProject.Accounts.Domain;
using PetProject.Core.Dtos.Accounts;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Contracts;

public interface IAccountsContract
{
    Task<Result<Permission[], ErrorList>> GetUserPermissions(long userId, CancellationToken cancellationToken = default);
    Task<Result<UserDto, ErrorList>> GetUserById(long userId, CancellationToken cancellationToken = default);
}