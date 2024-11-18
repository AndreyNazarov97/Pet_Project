using FluentValidation;

namespace PetProject.Application.Authorization.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        
    }
}