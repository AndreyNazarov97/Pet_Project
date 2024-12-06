using PetProject.Accounts.Domain;

namespace PetProject.Accounts.Application.Managers;

public interface IRefreshSessionManager
{
    public Task<RefreshSession?> GetByRefreshTokenAsync(Guid refreshToken, CancellationToken cancellationToken);
    void Delete(RefreshSession refreshSession);
}