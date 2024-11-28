using PetProject.Accounts.Application.AccountManagement.Commands.LoginUser;

namespace PetProject.Accounts.Presentation.Requests;

public record LoginRequest(string Email, string Password)
{
    public LoginUserCommand ToCommand() => new()
    {
        Email = Email,
        Password = Password
    };
}