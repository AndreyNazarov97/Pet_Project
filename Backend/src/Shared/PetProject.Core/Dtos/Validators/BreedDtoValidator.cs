using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Core.Dtos.Validators;

public class BreedDtoValidator : AbstractValidator<BreedDto>
{
    public BreedDtoValidator()
    {
        RuleFor(b => b.Name)
            .MustBeValueObject(BreedName.Create);
    }
}