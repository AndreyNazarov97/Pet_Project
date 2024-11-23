using FluentValidation;
using PetProject.Application.SpeciesManagement.GetSpeciesList;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.GetBreedsList;

public class GetBreedsListQueryValidator : AbstractValidator<GetBreedsListQuery>
{
    public GetBreedsListQueryValidator()
    {
        RuleFor(g => g.Limit)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Limit"));

        RuleFor(g => g.Offset)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Offset"));
    }
}