using PetProject.Application.Authorization.Commands.RegisterUser;

namespace PetProject.API.Controllers.Account.Requests;

public record RegisterUserRequest(string Email, string Username ,string Password)
{
    public RegisterUserCommand ToCommand() => new(Email, Username, Password);
}