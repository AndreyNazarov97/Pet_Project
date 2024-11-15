using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.Volunteers.DeleteVolunteer;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(d => d.Id)
            .NotEmpty()
            .WithError(Errors.General.LengthIsInvalid("Id"));
    }
}