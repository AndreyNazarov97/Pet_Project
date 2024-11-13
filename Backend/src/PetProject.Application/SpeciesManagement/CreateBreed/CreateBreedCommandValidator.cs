using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.CreateBreed;

public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleFor(c => c.BreedName)
            .MustBeValueObject(BreedName.Create);

        RuleFor(c => c.SpeciesName)
            .MustBeValueObject(SpeciesName.Create);
    }
}