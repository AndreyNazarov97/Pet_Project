using FluentValidation;

namespace PetProject.Accounts.Application.AccountManagement.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        
    }
}