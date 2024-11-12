using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.CreateSpecies;

public class CreateSpeciesRequestValidator : AbstractValidator<CreateSpeciesRequest>
{
    public CreateSpeciesRequestValidator()
    {
        RuleFor(c => c.Name)
            .MustBeValueObject(SpeciesName.Create);
    }
}