using FluentValidation;

namespace PetProject.Accounts.Application.AccountManagement.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        
    }
}