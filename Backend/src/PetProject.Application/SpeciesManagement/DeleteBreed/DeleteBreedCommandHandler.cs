using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.DeleteBreed;

public class DeleteBreedCommandHandler : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandHandler()
    {
        RuleFor(d => d.BreedName)
            .MustBeValueObject(BreedName.Create);
    }
}