using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;
using PetProject.Domain.SpeciesManagement.ValueObjects;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.VolunteersManagement.GetPets;

public class GetPetsQueryValidator : AbstractValidator<GetPetsQuery>
{
    public GetPetsQueryValidator()
    {
        RuleFor(g => g.Name)
            .MustBeValueObject(PetName.Create!)
            .When(p => p.Name is not null);

        RuleFor(g => g.MinAge)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid("Age"))
            .When(g => g.MinAge is not null);

        RuleFor(g => g.Breed)
            .MustBeValueObject(BreedName.Create!)
            .When(p => p.Breed is not null);
        
        RuleFor(g => g.Species)
            .MustBeValueObject(SpeciesName.Create!)
            .When(p => p.Species is not null);
        
        RuleFor(g => g.Limit)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Limit"));
        
        RuleFor(g => g.Offset)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Offset"));
    }
}