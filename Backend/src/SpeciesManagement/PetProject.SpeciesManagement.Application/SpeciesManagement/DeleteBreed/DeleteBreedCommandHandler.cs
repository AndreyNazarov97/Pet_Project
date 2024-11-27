using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.DeleteBreed;

public class DeleteBreedCommandHandler : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandHandler()
    {
        RuleFor(d => d.BreedName)
            .MustBeValueObject(BreedName.Create);
    }
}