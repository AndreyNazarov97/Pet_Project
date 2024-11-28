using FluentValidation;

namespace PetProject.Application.Authorization.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        
    }
}