using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Core.Dtos.Validators;

public class PetDtoValidator : AbstractValidator<PetDto>
{
    public PetDtoValidator()
    {
        
        RuleFor(p => p.PetName)
            .MustBeValueObject(PetName.Create!)
            .When(p => p.PetName is not null);
        
        RuleFor(p => p.GeneralDescription)
            .MustBeValueObject(Description.Create!)
            .When(p => p.GeneralDescription is not null);

        RuleFor(p => p.HealthInformation)
            .MustBeValueObject(Description.Create!)
            .When(p => p.HealthInformation is not null);
            

        RuleFor(p => p.SpeciesName)
            .MustBeValueObject(SpeciesName.Create!)
            .When(p => p.SpeciesName is not null);
        
        RuleFor(p => p.BreedName)
            .MustBeValueObject(BreedName.Create!)
            .When(p => p.BreedName is not null);

        RuleFor(p => p.Address)
            .SetValidator(new AddressDtoValidator()!)
            .When(p => p.Address is not null);
        
        RuleFor(p => new{p.Weight, p.Height})
            .MustBeValueObject(x => PetPhysicalAttributes.Create((double)x.Weight!, (double)x.Height!))
            .When(p => p.Weight is not null && p.Height is not null);

        RuleFor(p => p.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create!)
            .When(p => p.PhoneNumber is not null);
        
        RuleFor(c => c.BirthDate)
            .Must(birthDate => birthDate < DateTime.UtcNow)
            .When(p => p.BirthDate is not null)
            .WithMessage("Birth date must be in the past");
    }
}