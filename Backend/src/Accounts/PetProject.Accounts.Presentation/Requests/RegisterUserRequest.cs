using PetProject.Accounts.Application.AccountManagement.Commands.RegisterUser;
using PetProject.Core.Dtos;

namespace PetProject.Accounts.Presentation.Requests;

public record RegisterUserRequest(FullNameDto FullName ,string Email, string UserName ,string Password)
{
    public RegisterUserCommand ToCommand() => new()
    {
        FullName = FullName,
        Email = Email,
        UserName = UserName,
        Password = Password
    };
}