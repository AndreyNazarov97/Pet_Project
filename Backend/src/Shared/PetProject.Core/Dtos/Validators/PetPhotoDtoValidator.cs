using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Core.Dtos.Validators;

public class PetPhotoDtoValidator : AbstractValidator<PhotoDto>
{
    public PetPhotoDtoValidator()
    {
        RuleFor(p => p.Path)
            .MustBeValueObject(FilePath.Create);
    }
}