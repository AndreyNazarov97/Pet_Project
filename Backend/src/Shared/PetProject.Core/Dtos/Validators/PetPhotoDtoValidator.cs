using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace PetProject.Core.Dtos.Validators;

public class PetPhotoDtoValidator : AbstractValidator<PhotoDto>
{
    public PetPhotoDtoValidator()
    {
        RuleFor(p => p.FileId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("FileId"));
    }
}