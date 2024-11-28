using PetProject.Accounts.Application.AccountManagement.Commands.RegisterUser;

namespace PetProject.Accounts.Presentation.Requests;

public record RegisterUserRequest(string Email, string Username ,string Password)
{
    public RegisterUserCommand ToCommand() => new()
    {
        Email = Email,
        Username = Username,
        Password = Password
    };
}