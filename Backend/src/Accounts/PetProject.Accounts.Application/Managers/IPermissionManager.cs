using PetProject.Accounts.Domain;

namespace PetProject.Accounts.Application.Managers;

public interface IPermissionManager
{
    Task AddRangeIfExist(IEnumerable<string> permissions, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Permission>> GetUserPermissionsAsync(long userId, CancellationToken cancellationToken = default);

}