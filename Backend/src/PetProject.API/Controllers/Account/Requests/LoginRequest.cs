using PetProject.Application.Authorization.Commands.LoginUser;

namespace PetProject.API.Controllers.Account.Requests;

public record LoginRequest(string Email, string Password)
{
    public LoginUserCommand ToCommand() => new(Email, Password);
}