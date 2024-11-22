using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.SpeciesManagement.GetSpeciesList;

public class GetSpeciesListQueryValidator : AbstractValidator<GetSpeciesListQuery>
{
    public GetSpeciesListQueryValidator()
    {
        RuleFor(g => g.Limit)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Limit"));
        
        RuleFor(g => g.Offset)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Offset"));
    }
}