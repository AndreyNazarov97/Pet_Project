using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.Dto.Validators;

public class BreedDtoValidator : AbstractValidator<BreedDto>
{
    public BreedDtoValidator()
    {
        RuleFor(b => b.Name)
            .MustBeValueObject(BreedName.Create);
    }
}