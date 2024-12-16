using FluentValidation;
using PetProject.Core.Dtos.Validators;

namespace PetProject.Accounts.Application.AccountManagement.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FullName)
            .SetValidator(new FullNameDtoValidator());
    }
}