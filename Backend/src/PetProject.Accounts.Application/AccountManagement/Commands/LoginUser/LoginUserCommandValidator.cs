using FluentValidation;

namespace PetProject.Accounts.Application.AccountManagement.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        
    }
}