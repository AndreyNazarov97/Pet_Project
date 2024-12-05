using PetProject.Accounts.Domain;

namespace PetProject.Accounts.Application;

public interface ITokenProvider
{
    Task<string> GenerateAccessToken(User user);
}