using FluentValidation;
using PetProject.Core.Dtos.Validators;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.CreatePet;

public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(c => c.Address)
            .SetValidator(new AddressDtoValidator());
        
        RuleFor(c => c.Name)
            .MustBeValueObject(PetName.Create);

        RuleFor(c => c.GeneralDescription)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.HealthInformation)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.SpeciesName)
            .MaximumLength(100);

        RuleFor(c => c.BreedName)
            .MaximumLength(100);

        RuleFor(c => new{c.Height, c.Weight})
            .MustBeValueObject(x=> PetPhysicalAttributes.Create(x.Weight, x.Height));
        
        RuleFor(c => c.BirthDate)
            .Must(birthDate => birthDate < DateTime.UtcNow)
            .WithMessage("Birth date must be in the past");
    }
}