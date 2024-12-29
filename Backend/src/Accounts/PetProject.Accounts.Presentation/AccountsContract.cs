using CSharpFunctionalExtensions;
using PetProject.Accounts.Application.AccountManagement.Queries.GetUserInfo;
using PetProject.Accounts.Application.AccountManagement.Queries.GetUserPermissions;
using PetProject.Accounts.Contracts;
using PetProject.Accounts.Domain;
using PetProject.Core.Dtos.Accounts;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Presentation;

public class AccountsContract : IAccountsContract
{
    private readonly GetUserInfoHandler _getUserInfoHandler;
    private readonly GetUserPermissionsHandler _getUserPermissionsHandler;

    public AccountsContract(
        GetUserInfoHandler getUserInfoHandler,
        GetUserPermissionsHandler getUserPermissionsHandler)
    {
        _getUserInfoHandler = getUserInfoHandler;
        _getUserPermissionsHandler = getUserPermissionsHandler;
    }
    
    public async Task<Result<Permission[], ErrorList>> GetUserPermissions(long userId, CancellationToken cancellationToken = default)
    {
        var query = new GetUserPermissionsQuery() { UserId = userId };
        
        var result = await _getUserPermissionsHandler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }

    public async Task<Result<UserDto, ErrorList>> GetUserById(long userId, CancellationToken cancellationToken = default)
    {
        var query = new GetUserInfoQuery() { UserId = userId };
        
        var result = await _getUserInfoHandler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }
}