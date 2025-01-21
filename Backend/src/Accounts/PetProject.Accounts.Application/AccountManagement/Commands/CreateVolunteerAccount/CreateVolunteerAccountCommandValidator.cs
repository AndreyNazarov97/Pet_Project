using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Application.AccountManagement.Commands.CreateVolunteerAccount;

public class CreateVolunteerAccountCommandValidator : AbstractValidator<CreateVolunteerAccountCommand>
{
    public CreateVolunteerAccountCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithError(Errors.General.LengthIsInvalid("UserId"));

        RuleFor(x => x.Experience)
            .MustBeValueObject(Experience.Create);
    }
}