using PetProject.Accounts.Domain;

namespace PetProject.Accounts.Application;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}