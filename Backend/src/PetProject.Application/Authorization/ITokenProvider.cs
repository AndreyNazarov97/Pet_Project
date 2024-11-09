using PetProject.Application.Authorization.DataModels;

namespace PetProject.Application.Authorization;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}