using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.DeleteSpecies;

public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesCommandValidator()
    {
        RuleFor(d => d.SpeciesName)
            .MustBeValueObject(SpeciesName.Create);
    }
}