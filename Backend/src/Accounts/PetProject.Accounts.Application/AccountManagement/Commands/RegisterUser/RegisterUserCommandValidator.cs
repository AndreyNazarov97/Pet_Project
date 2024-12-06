using FluentValidation;
using PetProject.Core.Dtos.Validators;

namespace PetProject.Accounts.Application.AccountManagement.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FullName)
            .SetValidator(new FullNameDtoValidator());
    }
}