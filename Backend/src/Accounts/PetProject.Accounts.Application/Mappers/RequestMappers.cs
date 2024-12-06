using PetProject.Accounts.Application.AccountManagement.Commands.LoginUser;
using PetProject.Accounts.Application.AccountManagement.Commands.RegisterUser;
using PetProject.Accounts.Application.AccountManagement.Commands.UpdateRequisites;
using PetProject.Accounts.Application.AccountManagement.Commands.UpdateSocialNetworks;
using PetProject.Accounts.Contracts.Requests;

namespace PetProject.Accounts.Application.Mappers;

public static class RequestMappers
{
    public static LoginUserCommand ToCommand(this LoginRequest request) => new()
    {
        Email = request.Email,
        Password = request.Password
    };

    public static RegisterUserCommand ToCommand(this RegisterUserRequest request) => new()
    {
        Email = request.Email,
        UserName = request.UserName,
        Password = request.Password,
        FullName = request.FullName
    };

    public static UpdateRequisitesCommand ToCommand(this UpdateRequisitesRequest request, long userId) => new()
    {
        UserId = userId,
        Requisites = request.Requisites
    };

    public static UpdateSocialNetworksCommand ToCommand(this UpdateSocialNetworksRequest request, long userId) => new()
    {
        UserId = userId,
        SocialLinks = request.SocialLinks
    };
}