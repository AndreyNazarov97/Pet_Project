using FluentValidation;
using PetProject.Application.Dto.Validators;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.UpdateRequisites;

public class UpdateRequisitesCommandValidator : AbstractValidator<UpdateRequisitesCommand>
{
    public UpdateRequisitesCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.LengthIsInvalid("Id"));

        RuleForEach(x => x.Requisites)
            .SetValidator(new RequisiteDtoValidator());
    }
}