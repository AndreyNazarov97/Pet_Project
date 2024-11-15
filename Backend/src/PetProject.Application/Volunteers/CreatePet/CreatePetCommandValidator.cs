using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Volunteers.CreatePet;

public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(c => new { c.Address.Country, c.Address.City, c.Address.Street, c.Address.House, c.Address.Flat })
            .MustBeValueObject(x => Address.Create(x.Country, x.City, x.Street, x.House, x.Flat));

        RuleFor(c => c.Name)
            .MustBeValueObject(PetName.Create);

        RuleFor(c => c.GeneralDescription)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.HealthInformation)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.Species)
            .MaximumLength(100);

        RuleFor(c => c.Breed)
            .MaximumLength(100);

        RuleFor(c => new{c.Height, c.Weight})
            .MustBeValueObject(x=> PetPhysicalAttributes.Create(x.Weight, x.Height));
        
        RuleFor(c => c.BirthDate)
            .Must(birthDate => birthDate.ToDateTime(TimeOnly.MinValue) < DateTime.UtcNow)
            .WithMessage("Birth date must be in the past");
    }
}