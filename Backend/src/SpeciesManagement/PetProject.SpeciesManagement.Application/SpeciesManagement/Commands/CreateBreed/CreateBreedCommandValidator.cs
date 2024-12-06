using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.CreateBreed;

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