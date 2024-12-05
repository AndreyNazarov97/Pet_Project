using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Queries.GetUserPermissions;

public class GetUserPermissionsHandler : IRequestHandler<GetUserPermissionsQuery, Result<Permission[], ErrorList>>
{
    private readonly IPermissionManager _permissionManager;

    public GetUserPermissionsHandler(
        IPermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }
    
    public async Task<Result<Permission[], ErrorList>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var permissions = await _permissionManager
                .GetUserPermissionsAsync(request.UserId, cancellationToken);
            
            return permissions.ToArray();
        }
        catch (Exception e)  
        {
            return Error.Failure("could.not.get.user.permissions", e.Message).ToErrorList();
        }
    }
}