using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Core.Dtos.Validators;

public class PetPhotoDtoValidator : AbstractValidator<PetPhotoDto>
{
    public PetPhotoDtoValidator()
    {
        RuleFor(p => p.Path)
            .MustBeValueObject(FilePath.Create);
    }
}